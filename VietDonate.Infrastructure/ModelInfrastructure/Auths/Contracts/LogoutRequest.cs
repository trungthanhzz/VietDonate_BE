using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Infrastructure.ModelInfrastructure.Auths.Contracts
{
    public record LogoutRequest(string RefreshToken);
}
