using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler(
        IUserRepository userRepository)
        : IQueryHandler<GetUsersQuery, Result<GetUsersResult>>
    {
        private const int MaxPageSize = 100;
        private const int DefaultPageSize = 20;

        public async Task<Result<GetUsersResult>> Handle(
            GetUsersQuery query,
            CancellationToken cancellationToken)
        {
            // Validate page number
            if (query.Page < 1)
            {
                return Result.Failure<GetUsersResult>(GetUsersErrors.InvalidPageNumber);
            }

            // Validate and normalize page size
            var pageSize = query.PageSize;
            if (pageSize < 1)
            {
                pageSize = DefaultPageSize;
            }
            else if (pageSize > MaxPageSize)
            {
                return Result.Failure<GetUsersResult>(GetUsersErrors.InvalidPageSize);
            }

            // Get paginated users with filters
            var (users, totalCount) = await userRepository.GetPagedAsync(
                query.Page,
                pageSize,
                query.Role,
                query.Email,
                query.Name,
                cancellationToken);

            // Calculate pagination metadata
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var hasPreviousPage = query.Page > 1;
            var hasNextPage = query.Page < totalPages;

            // Map to result items
            var userItems = users.Select(u => new UserItem(
                Id: u.Id,
                UserName: u.UserName,
                FullName: u.UserInformation?.FullName ?? string.Empty,
                Email: u.UserInformation?.Email,
                Phone: u.UserInformation?.Phone,
                Address: u.UserInformation?.Address,
                AvtUrl: u.UserInformation?.AvtUrl,
                CreatedDate: u.CreatedDate,
                Role: u.RoleType.ToString(),
                Status: u.UserInformation?.Status,
                VerificationStatus: u.UserInformation?.VerificationStatus
            )).ToList();

            var paginationMetadata = new PaginationMetadata(
                Page: query.Page,
                PageSize: pageSize,
                TotalCount: totalCount,
                TotalPages: totalPages,
                HasPreviousPage: hasPreviousPage,
                HasNextPage: hasNextPage
            );

            var result = new GetUsersResult(
                Users: userItems,
                Pagination: paginationMetadata
            );

            return Result.Success(result);
        }
    }
}
