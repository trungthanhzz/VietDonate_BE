using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserPartial
{
    public record UpdateUserPartialCommand(
        string? FullName,
        string? Phone,
        string? Email,
        string? Address,
        string? AvtUrl,
        DateTime? DateOfBirth,
        string? Status,
        string? VerificationStatus,
        string? IdentityNumber,
        string? OrganizationName,
        string? OrganizationTaxCode,
        string? OrganizationRegisterNumber,
        string? OrganizationLegalRepresentative,
        string? BankAccountNumber,
        string? BankName,
        string? BankBranch,
        string? StaffNumber
    ) : ICommand<Result<UpdateUserPartialResult>>;
}

