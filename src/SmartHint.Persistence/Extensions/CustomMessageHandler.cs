namespace SmartHint.Persistence.Extensions;

public static class CustomMessageHandler
{
    public static string EntityNotFound(string entityName, int id)
    {
        return $"{entityName} não encontrado(a) com o ID {id}.";
    }

    public static string UnexpectedError(string operation, string message)
    {
        return $"Erro inesperado ao {operation}: {message}";
    }

    public static string DbUpdateError(DbUpdateException exception)
    {
        return DbUpdateExceptionHandler.HandleDbUpdateException(exception);
    }
}
