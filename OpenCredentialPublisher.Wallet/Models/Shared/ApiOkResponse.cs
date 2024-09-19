namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result, string message = null, string redirectUrl = null)
            : base(200, message, redirectUrl)
        {
            Result = result;

        }
    }
}
