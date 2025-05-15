namespace Clean.Solutions.Vertical.Shared
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new(
            "Error",
            "A validation problem occurred.");

        Error[] Errors { get; }
    }

    public sealed class ValidationResult : Result, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(false, IValidationResult.ValidationError) =>
            Errors = errors;

        public Error[] Errors { get; }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }

    public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(default, false, IValidationResult.ValidationError) =>
            Errors = errors;

        public Error[] Errors { get; }

        public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}
