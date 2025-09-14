using System.Diagnostics.CodeAnalysis;

namespace VietDonate.Application.Common.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }
        protected Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error != null && error != Error.None)
                throw new InvalidOperationException("A successful result cannot have an error.");
            if (!isSuccess && (error == null || error == Error.None))
                throw new InvalidOperationException("A failure result must have an error.");
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue>(TValue? value, bool isSuccess, Error? error) : Result(isSuccess, error)
    {
        [NotNull]
        public TValue? Value => IsSuccess 
            ? value 
            : throw new InvalidOperationException("Cannot access the value of a failure result.");

        public static implicit operator Result<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.None);
        public static Result<TValue> ValidationFailure(Error error) => new(default, false, error);
    }
}
