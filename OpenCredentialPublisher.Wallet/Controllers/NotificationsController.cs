using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCredentialPublisher.Shared.Extensions;
using OpenCredentialPublisher.Wallet.Models.Notifications;

namespace OpenCredentialPublisher.Wallet.Controllers
{

    public class NotificationsController : SecureApiControllerBase<NotificationsController>
    {
        private readonly NotificationService _notificationService;
        private readonly ETLService _etlService;

        public NotificationsController(UserManager<ApplicationUser> userManager, 
            ILogger<NotificationsController> logger, 
            NotificationService notificationService,
            ETLService etlService) : base(userManager, logger)
        {
            _notificationService = notificationService;
            _etlService = etlService;
        }

        /// <summary>
        /// Get All pending Notifications for the user.
        /// GET api/notifications
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NotificationResponseModel>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> Get()
        {
            var notificationViewModels = await _notificationService.GetNotificationsAsync(_userId);
            
            return ApiOk(notificationViewModels);
        }

        /// <summary>
        /// Marking notification as read 
        /// PUT api/notifications/{notificationId}/read
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpPut("{notificationId}/read")]
        [ProducesResponseType(200, Type = null)]  /* success returns 200 - Ok */
        public async Task<IActionResult> MarkNotificationRead(int notificationId)
        {
            try
            {
                var notification = await _notificationService.GetNotificationAsync(notificationId, _userId);

                //ensure we have a notification.
                if (notification != null)
                {
                    //clean up notification (soft delete it).
                    await _notificationService.MarkAsReadAsync(notification);
                }

                //all okay.
                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem marking notification as read {0}", notificationId);

                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Marking notification as read 
        /// PUT api/notifications/{notificationId}/read
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpPut("{notificationId}/unread")]
        [ProducesResponseType(200, Type = null)]  /* success returns 200 - Ok */
        public async Task<IActionResult> MarkNotificationUnRead(int notificationId)
        {
            try
            {
                var notification = await _notificationService.GetNotificationAsync(notificationId, _userId);

                //ensure we have a notification.
                if (notification != null)
                {
                    //clean up notification (soft delete it).
                    await _notificationService.MarkAsUnReadAsync(notification);
                }

                //all okay.
                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem marking notification as unread {0}", notificationId);

                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        /// <summary>
        /// Adding credentital to the system.
        /// POST api/notifications/{notificationId}/credential 
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpPost("{notificationId}/credential")]
        [ProducesResponseType(200, Type = null)]  /* success returns 200 - Ok */
        public async Task<IActionResult> AddCredentialFromNotification(int notificationId)
        {
            try
            {
                var notification = await _notificationService.GetNotificationAsync(notificationId, _userId);

                //ensure we have a notification.
                if (notification != null)
                {
                    var result = await _etlService.ProcessJson(this, _userId,
                        notification.Json, null);

                    //if we had an error processing return with known error message.
                    if (result.HasError)
                    {
                        foreach (var err in result.ErrorMessages)
                        {
                            ModelState.AddModelError("AddCredentialFromNotification", err);

                            //if the credential has already been added then mark it as a soft delete. 
                            //but still return error.
                            if (ETLService.CREDENTIAL_ALREADY_LOADED.Equals(err))
                            {
                                await _notificationService.RemoveNotificationAsync(notification);
                                return ApiOk(null);
                            }
                        }
                    }

                    if (!ModelState.IsValid) 
                        return ApiOkModelInvalid(ModelState);

                    //clean up notification (soft delete it).
                    await _notificationService.RemoveNotificationAsync(notification);
                }

                //all okay.
                return ApiOk(null);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem adding credential from notification notificationId {0}", notificationId);

                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
