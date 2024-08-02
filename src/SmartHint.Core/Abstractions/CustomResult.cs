namespace SmartHint.Core.Abstractions;

public enum ECustomResultStatus
{
    Success,
    HasValidation,
    HasError,
    EntityNotFound,
    EntityHasError,
    EntityAlreadyExists,
    NoContent
}

public interface ICustomResult
{
    ECustomResultStatus Status { get; }
}

public interface ICustomResult<out T> : ICustomResult
{
    T? Data { get; }
}

public interface ICustomResultValidations : ICustomResult
{
    IEnumerable<Validation> Validations { get; }
}

public interface ICustomResultError : ICustomResult
{
    Error? Error { get; }
}

public interface IRequestEntityWarning : ICustomResult
{
    EntityWarning? EntityWarning { get; }
}

public class CustomResult : ICustomResultValidations, ICustomResultError, IRequestEntityWarning
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Message { get; set; } = string.Empty;

    public ECustomResultStatus Status { get; set; }
    public List<string> GeneralErrors { get; set; } = new();
    public Dictionary<string, List<string>> EntityErrors { get; set; } = new();

    public CustomResult()
    {
        Status = ECustomResultStatus.Success;
    }

    public CustomResult(string message)
    {
        AddError(message);
    }

    public CustomResult(string id, string message)
    {
        Id = id;
        Message = message;
    }

    public void AddError(string message)
    {
        Status = ECustomResultStatus.HasError;
        GeneralErrors.Add(message);
    }

    public void AddEntityError(string entity, string message)
    {
        Status = ECustomResultStatus.EntityHasError;
        if (!EntityErrors.TryGetValue(entity, out var value))
        {
            value = new List<string>();
            EntityErrors[entity] = value;
        }
        value.Add(message);
    }

    public override string ToString()
    {
        var messages = new List<string>();

        if (GeneralErrors.Count != 0)
        {
            messages.AddRange(GeneralErrors);
        }

        foreach (var entityError in EntityErrors)
        {
            foreach (var error in entityError.Value)
            {
                messages.Add($"{entityError.Key}: {error}");
            }
        }
        return string.Join("; ", messages);
    }

    public static CustomResult Success() => new CustomResult { Status = ECustomResultStatus.Success };
    public static CustomResult WithNoContent() => new CustomResult { Status = ECustomResultStatus.NoContent };
    public static CustomResult EntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomResult EntityHasError(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomResult EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomResult WithError(string message)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = new Error(message)
        };
    public static CustomResult WithError(Exception exception) => WithError(exception.Message);

    public static CustomResult WithError(List<string> generalErrors)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            GeneralErrors = generalErrors
        };

    public static CustomResult WithError(Dictionary<string, List<string>> entityErrors)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityErrors = entityErrors
        };

    public static CustomResult WithError(Error error)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = error
        };
    public static CustomResult WithValidations(params Validation[] validations)
        => new()
        {
            Status = ECustomResultStatus.HasValidation,
            Validations = validations
        };
    public static CustomResult WithValidations(IEnumerable<Validation> validations)
        => WithValidations(validations.ToArray());
    public static CustomResult WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public IEnumerable<Validation> Validations { get; protected init; } = Enumerable.Empty<Validation>();
    public Error? Error { get; protected init; }
    public EntityWarning? EntityWarning { get; protected init; }
}

public class CustomResult<T> : CustomResult, ICustomResult<T>
{
    public T? Data { get; private init; }

    public static CustomResult<T> Success(T data)
        => new()
        {
            Data = data,
            Status = ECustomResultStatus.Success
        };

    public new static CustomResult<T> WithNoContent()
        => new()
        {
            Status = ECustomResultStatus.NoContent
        };

    public new static CustomResult<T> EntityNotFound(string entity, object? id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    public new static CustomResult<T> EntityHasError(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    public new static CustomResult<T> EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    public new static CustomResult<T> WithError(string message)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = new Error(message)
        };

    public new static CustomResult<T> WithError(Exception exception) => WithError(exception.Message);

    public new static CustomResult<T> WithError(List<string> generalErrors)
       => new()
       {
           Status = ECustomResultStatus.HasError,
           GeneralErrors = generalErrors
       };

    public new static CustomResult<T> WithError(Dictionary<string, List<string>> entityErrors)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityErrors = entityErrors
        };

    public new static CustomResult<T> WithError(Error error)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = error
        };

    public new static CustomResult<T> WithValidations(params Validation[] validations)
        => new()
        {
            Status = ECustomResultStatus.HasValidation,
            Validations = validations
        };

    public new static CustomResult<T> WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public static implicit operator CustomResult<T>(T data) => Success(data);
    public static implicit operator CustomResult<T>(Exception ex) => WithError(ex);
    public static implicit operator CustomResult<T>(Validation[] validations) => WithValidations(validations);
    public static implicit operator CustomResult<T>(Validation validation) => WithValidations(validation);
}

public record Validation(string PropertyName, string Description);
public record Error(string Description);
public record EntityWarning(string Name, object? Id, string Message);
