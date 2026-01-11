namespace VietDonate.Application.UseCases.Users.Queries.GetUsers
{
    public record UserItem(
        Guid Id,
        string UserName,
        string FullName,
        string? Email,
        string? Phone,
        string? Address,
        string? AvtUrl,
        DateTime CreatedDate,
        string Role,
        string? Status,
        string? VerificationStatus
    );

    public record PaginationMetadata(
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage
    );

    public record GetUsersResult(
        List<UserItem> Users,
        PaginationMetadata Pagination
    );
}
