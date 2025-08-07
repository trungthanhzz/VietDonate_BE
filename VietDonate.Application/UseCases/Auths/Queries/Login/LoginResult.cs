using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public record LoginResult(
        string AccessToken, 
        string RefreshToken, 
        int ExpireDate);
}
