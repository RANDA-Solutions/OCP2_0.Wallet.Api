using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public record CredentialDetailsAlignmentResponseModel
    {
        protected CredentialDetailsAlignmentResponseModel(AchievementAlignment achievementAlignment)
        {
            TargetName = achievementAlignment.TargetName;
            TargetUrl = achievementAlignment.TargetUrl;
        }

        public string TargetName { get;  }
        public string TargetUrl { get;  }

        public static CredentialDetailsAlignmentResponseModel FromModel(AchievementAlignment achievementAlignment)
        {
            return new CredentialDetailsAlignmentResponseModel(achievementAlignment);
        }

    }
}