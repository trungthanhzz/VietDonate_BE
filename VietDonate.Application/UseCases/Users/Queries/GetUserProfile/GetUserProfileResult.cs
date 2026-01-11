namespace VietDonate.Application.UseCases.Users.Queries.GetUserProfile
{
    public record GetUserProfileResult(
        Guid Id,
        string UserName,
        string FullName,
        string? Email,
        string? Phone,
        string Address,
        string AvtUrl,
        DateTime CreatedDate,
        DateTime? DateOfBirth,
        string? Status,
        string? VerificationStatus,
        decimal TotalDonated,
        decimal TotalRecieved,
        int CampaignCount,
        string Role);
}

