using OpenCredentialPublisher.Data.Models;
using System;
using OpenCredentialPublisher.Shared.Interfaces;
using IdentityModel;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Data.Models.Enums;
using System.Text;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{

    public class Setup: IBaseEntity
    {
        public const int VALID_UNTIL_HOURS = 24;

        public long SetupId { get; set; }

        public string UserId { get; set; }
        public string AccessCode { get; set; }

        public long? MessageId { get; set; }
        public Message Message { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset ValidUntil { get; set; }
        public ApplicationUser User { get; set; }

        public void GenerateAccessCodeAndMessage()
        {
            AccessCode = AccessCodeGenerator.GenerateUniqueNumericCode(CryptoRandom.CreateRandomKey(10));
            ValidUntil = DateTimeOffset.UtcNow.AddHours(VALID_UNTIL_HOURS);
            ModifiedAt = DateTimeOffset.UtcNow;

            var bodyStringBuilder = new StringBuilder();

            bodyStringBuilder.AppendLine("<p>Enter the access code below to complete setting up your account:</p>");
            bodyStringBuilder.AppendLine($"<p style=\"text-align:center;font-size:24pt; font-weight:bold;\">{AccessCode}</p>");
            bodyStringBuilder.AppendLine($"<p style=\"font-size:12px;font-color:#666666\">This code expires in {VALID_UNTIL_HOURS} hours.</p>");

           Message = new Message
            {
                Body = bodyStringBuilder.ToString(),
                Recipient = User.Email,
                Subject = $"{AccessCode} is your Kentucky.gov Learning & Employment Wallet access code",
                SendAttempts = 0,
                StatusId = StatusEnum.Created
            };
        }

        public AccessCodeStatusEnum VerifyCode(string email, string accessCode)
        {
            if (User.Email != null
                && User.Email.Equals(email,StringComparison.OrdinalIgnoreCase)
                && AccessCode == accessCode)
            {
                if (ValidUntil >= DateTimeOffset.UtcNow)
                {
                    return AccessCodeStatusEnum.Valid;
                }

                return AccessCodeStatusEnum.Expired;
            }
            return AccessCodeStatusEnum.Invalid;
        }
    }
}
