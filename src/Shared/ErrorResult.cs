namespace Clean.Solutions.Vertical.Shared
{
    public interface IErrorResult
    {
        public static readonly Error Error = new(
            "Error",
            "An error has occurred.");

        Error[] Errors { get; }
    }

    public sealed class ErrorResult : Result, IErrorResult
    {
        private ErrorResult(Error[] errors)
            : base(false, IErrorResult.Error) =>
            Errors = errors;

        public Error[] Errors { get; }

        public static ErrorResult WithErrors(Error[] errors) => new(errors);
    }

    public sealed class ErrorResult<TValue> : Result<TValue>, IErrorResult
    {
        private ErrorResult(Error[] errors)
            : base(default, false, IErrorResult.Error) =>
            Errors = errors;

        public Error[] Errors { get; }

        public static ErrorResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}
