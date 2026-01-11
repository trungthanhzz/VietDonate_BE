using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.API.Utils.Extensions;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.UseCases.Users.Commands.Register;
using VietDonate.Application.UseCases.Users.Commands.UpdateUser;
using VietDonate.Application.UseCases.Users.Commands.UpdateUserPartial;
using VietDonate.Application.UseCases.Users.Commands.UpdateUserRole;
using VietDonate.Application.UseCases.Users.Queries.GetUserProfile;
using VietDonate.Application.UseCases.Users.Queries.GetUsers;
using VietDonate.Domain.Common;
using VietDonate.Infrastructure.ModelInfrastructure.Users.Contracts;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender mediator, ILogger<UserController> logger) : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!request.IsValid)
            {
                return BadRequest(new { Message = "At least one contact method (phone or email) is required." });
            }

            var command = new RegisterUserCommand(
                request.UserName,
                request.Password,
                request.FullName,
                request.Phone,
                request.Email,
                request.Address
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: registerResult => Ok(registerResult),
                onFailure: Problem
            );
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetUserProfileQuery();
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: profileResult => Ok(profileResult),
                onFailure: Problem
            );
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (!request.IsValid)
            {
                return BadRequest(new { Message = "At least one contact method (phone or email) is required." });
            }

            var command = new UpdateUserCommand(
                request.FullName,
                request.Phone,
                request.Email,
                request.Address,
                request.AvtUrl,
                request.DateOfBirth,
                request.Status,
                request.VerificationStatus,
                request.IdentityNumber,
                request.OrganizationName,
                request.OrganizationTaxCode,
                request.OrganizationRegisterNumber,
                request.OrganizationLegalRepresentative,
                request.BankAccountNumber,
                request.BankName,
                request.BankBranch,
                request.StaffNumber
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult => Ok(updateResult),
                onFailure: Problem
            );
        }

        [HttpPatch]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        [Route("profile")]
        public async Task<IActionResult> UpdateUserPartial([FromBody] UpdateUserPartialRequest request)
        {
            var command = new UpdateUserPartialCommand(
                request.FullName,
                request.Phone,
                request.Email,
                request.Address,
                request.AvtUrl,
                request.DateOfBirth,
                request.Status,
                request.VerificationStatus,
                request.IdentityNumber,
                request.OrganizationName,
                request.OrganizationTaxCode,
                request.OrganizationRegisterNumber,
                request.OrganizationLegalRepresentative,
                request.BankAccountNumber,
                request.BankName,
                request.BankBranch,
                request.StaffNumber
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult => Ok(updateResult),
                onFailure: Problem
            );
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        [Route("{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(
            [FromRoute] Guid userId,
            [FromBody] UpdateUserRoleRequest request)
        {
            var currentUserId = Request.GetUserIdAsGuid();
            var ipAddress = Request.GetIpAddress();
            var userAgent = Request.GetUserAgent();

            if (!Enum.TryParse<RoleType>(request.NewRole, ignoreCase: true, out var roleType))
            {
                return BadRequest(new { Message = $"Invalid role value: {request.NewRole}. Valid values are: Guest, User, Staff, Admin" });
            }

            var command = new UpdateUserRoleCommand(
                userId,
                roleType
            );

            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: updateResult =>
                {
                    logger.LogInformation(
                        "UpdateUserRole completed successfully. TargetUserId: {TargetUserId}, OldRole: {OldRole}, NewRole: {NewRole}, CurrentUserId: {CurrentUserId}",
                        userId, updateResult.OldRole, updateResult.NewRole, currentUserId);
                    return Ok(updateResult);
                },
                onFailure: errors =>
                {
                    logger.LogWarning(
                        "UpdateUserRole failed. TargetUserId: {TargetUserId}, NewRole: {NewRole}, CurrentUserId: {CurrentUserId}, Errors: {Errors}",
                        userId, roleType, currentUserId, errors);
                    return Problem(errors);
                }
            );
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        [Route("")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? role = null,
            [FromQuery] string? email = null,
            [FromQuery] string? name = null)
        {
            RoleType? roleType = null;
            if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse<RoleType>(role, ignoreCase: true, out var parsedRole))
            {
                roleType = parsedRole;
            }

            var query = new GetUsersQuery(
                Page: page,
                PageSize: pageSize,
                Role: roleType,
                Email: email,
                Name: name);

            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: usersResult => Ok(usersResult),
                onFailure: Problem
            );
        }
    }
}
