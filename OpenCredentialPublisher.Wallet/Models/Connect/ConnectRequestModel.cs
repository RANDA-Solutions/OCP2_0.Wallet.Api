using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.Data.Custom.Commands;

namespace OpenCredentialPublisher.Wallet.Models.Connect
{
    public class ConnectRequestModel
    {
        [FromQuery]
        public string Endpoint { get; set; }
        [FromQuery]
        public string Scope { get; set; }
        [FromQuery]
        public string Payload { get; set; }
        [FromQuery]
        public string Issuer { get; set; }
        [FromQuery]
        public string Method { get; set; }

        public ConnectRequestCommand ToCommand()
        {
            return new ConnectRequestCommand(Endpoint, Scope, Payload, Issuer, Method);
        }
    }
}
