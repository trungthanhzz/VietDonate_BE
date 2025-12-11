using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Infrastructure.ModelInfrastructure.Base;

namespace VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts
{
    public record UserRegistrationResponse(bool Status, string Message) : Response(Status, Message);
}
