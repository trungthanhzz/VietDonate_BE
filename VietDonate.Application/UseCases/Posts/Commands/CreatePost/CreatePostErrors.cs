using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.CreatePost
{
    public static class CreatePostErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to create a post");
        public static readonly Error TitleRequired = new(ErrorType.Validation, "Post title is required");
        public static readonly Error ContentRequired = new(ErrorType.Validation, "Post content is required");
        public static readonly Error PostTypeRequired = new(ErrorType.Validation, "Post type is required");
        public static readonly Error InvalidPostType = new(ErrorType.Validation, "Invalid post type");
    }
}
