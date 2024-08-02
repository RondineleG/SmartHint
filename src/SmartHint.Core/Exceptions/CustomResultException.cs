namespace SmartHint.Core.Exceptions;

public class CustomResultException : Exception
{
    public CustomResult CustomResult { get; }

    public CustomResultException(CustomResult customResult)
    {
        CustomResult = customResult;
    }

    public CustomResultException(params Validation[] validations)
        : this(CustomResult.WithValidations(validations)) { }

    public CustomResultException(Exception exception)
        : this(CustomResult.WithError(exception)) { }
}
