namespace SmartHint.API.Extensions;

public static class ModelStateExtensions
{
    public static List<string> GetErrorMessages(this ModelStateDictionary dictionary)
    {
        return dictionary.SelectMany(m => m.Value.Errors).Select(m => m.ErrorMessage).ToList();
    }
}
