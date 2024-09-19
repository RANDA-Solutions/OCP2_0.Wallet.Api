using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Custom.Commands
{
    public record ConnectRequestCommand(
        string Endpoint,
        string Scope,
        string Payload,
        string Issuer,
        string Method);
}
