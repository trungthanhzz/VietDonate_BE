using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Tokens.Queries.Generate;

namespace VietDonate.API.Controllers
{
    [Route("tokens")]
    [AllowAnonymous]
    public class TestTokenController : ApiController
    {
        readonly ISender _mediator;
        public TestTokenController(ISender mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateToken(GenerateTokenQuery request)
        {

            var query = new GenerateTokenQuery(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Permissions,
                request.Roles);

            var result = await _mediator.Send(query);
            
            return result.Match(
                generateTokenResult => Ok(generateTokenResult.Token),
                Problem);
        }
    }
}
