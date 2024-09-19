using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Wallet.Models.Profile
{
    public class UserProfileResponseModel
    {
        public bool HasProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool MissingDisplayName => String.IsNullOrEmpty(DisplayName);
        public Int32 Credentials { get; set; }
        public Int32 Achievements { get; set; }
        public Int32 Scores { get; set; }
        public Int32 ActiveLinks { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }
    }
}
