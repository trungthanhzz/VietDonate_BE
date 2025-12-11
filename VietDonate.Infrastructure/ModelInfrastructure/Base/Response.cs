using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Infrastructure.ModelInfrastructure.Base
{
    public record Response(
        bool Status,
        string Message);
}
