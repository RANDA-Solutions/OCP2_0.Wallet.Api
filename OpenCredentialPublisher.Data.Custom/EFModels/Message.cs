using System;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public class Message : IBaseEntity
    {
        public long Id { get; set; }
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public int SendAttempts { get; set; } 
        public StatusEnum StatusId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTime.UtcNow;
        }

        public void MarkSent()
        {
            StatusId = StatusEnum.Sent;
            SendAttempts++;
            ModifiedAt = DateTimeOffset.UtcNow;
        }
    }
}
