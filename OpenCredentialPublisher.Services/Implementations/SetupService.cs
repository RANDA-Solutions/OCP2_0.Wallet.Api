using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class SetupService
    {
        private readonly WalletDbContext _context;
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetupService(WalletDbContext context,
            EmailService emailService, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<Setup> GenerateAndSendAccessCodeAsync(ApplicationUser user)
        {
            var (setup,message) = await GetOrCreateSetupAndMessageAsync(user);

            await _context.SaveChangesAsync();

            // send it out
            await _emailService.SendEmailAsync(message.Recipient,
                message.Subject,
                message.Body,
                true);

            message.MarkSent();
            _context.Messages.Update(message);

            await _context.SaveChangesAsync();

            return setup;
        }

        public Task<Setup> GetByEmailAsync(string email)
        {
            return _context.Setups
                .Include(sl => sl.User)
                .Include(sl => sl.Message)
                .FirstOrDefaultAsync(sl => sl.User.Email == email);
        }

        public async Task<AccessCodeStatusEnum> VerifyAccessCodeAsync(string email, string accessCode)
        {
            // find our setup link by email
            var setup = await GetByEmailAsync(email);

            if (setup == null)
                return AccessCodeStatusEnum.NotFound;

            // check that user is already set up
            if (setup.User.EmailConfirmed && !string.IsNullOrEmpty(setup.User.PasswordHash))
                return AccessCodeStatusEnum.AccountComplete;

            // check email and access code valid
            var accessCodeStatus = setup.VerifyCode(email, accessCode);

            await _context.SaveChangesAsync();

            return accessCodeStatus;
        }

        private async Task<(Setup, Message)> GetOrCreateSetupAndMessageAsync(ApplicationUser user)
        {
            var setup = await _context.Setups
                .Include(sl => sl.User)
                .Include(sl => sl.Message)
                .FirstOrDefaultAsync(sl => sl.UserId == user.Id);

            if (setup == null)
            {
                setup = new Setup
                {
                    User = user
                };

                await _context.Setups.AddAsync(setup);
            }

            setup.GenerateAccessCodeAndMessage();
            await _context.Messages.AddAsync(setup.Message);

            return (setup, setup.Message);
        }

        public async Task<AccessCodeStatusEnum> UpdateUserAsync(string email, string accessCode, string displayName, string password, ModelStateDictionary modelState)
        {
            // find our setup link by email
            var setup = await GetByEmailAsync(email);

            if (setup == null)
                return AccessCodeStatusEnum.NotFound;

            // check that user is already set up
            if (setup.User.EmailConfirmed && !string.IsNullOrEmpty(setup.User.PasswordHash))
                return AccessCodeStatusEnum.AccountComplete;

            // check email and access code valid
            var accessCodeStatus = setup.VerifyCode(email, accessCode);

            // if valid then update user information
            if (accessCodeStatus == AccessCodeStatusEnum.Valid)
            {
                //update user display name, email confirmed, and password
                setup.User.DisplayName = displayName;
                setup.User.EmailConfirmed = true;
                var addPasswordResult = await _userManager.AddPasswordAsync(setup.User, password);

                if (!addPasswordResult.Succeeded)
                {
                    foreach (var identityError in addPasswordResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(string.Empty,errorDescription);
                    }

                    return AccessCodeStatusEnum.AccountUpdateFailed;
                }
                
                var updateResult = await _userManager.UpdateAsync(setup.User);

                if (!updateResult.Succeeded)
                {
                    foreach (var identityError in updateResult.Errors)
                    {
                        var errorDescription = identityError.Description;
                        if (!string.IsNullOrEmpty(errorDescription))
                            modelState.AddModelError(string.Empty, errorDescription);
                    }

                    return AccessCodeStatusEnum.AccountUpdateFailed;
                }


                // link is used so delete
                setup.Delete();

                await _context.SaveChangesAsync();

                return AccessCodeStatusEnum.AccountComplete;
            }

            return accessCodeStatus;
        }
    }
}