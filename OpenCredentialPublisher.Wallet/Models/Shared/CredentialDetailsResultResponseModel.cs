using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public record CredentialDetailsResultResponseModel
    {
        protected CredentialDetailsResultResponseModel(Result result)
        {
            ResultType = result.ResultDescriptionType ?? "Unspecified";
            Status = result.Status ?? "Unspecified";
            Value = result.Value ?? "Unspecified";
        }

        public string ResultType { get;  }
        public string Value { get;  }
        public string Status { get; }

        public static CredentialDetailsResultResponseModel FromModel(Result result)
        {
            return new CredentialDetailsResultResponseModel(result);
        }

    }
}