using SmartHint.Application.UseCases.Compradores;

namespace SmartHint.API.Extensions;

public static class UseCasesServicesExtensions
{
    public static void RegisterUseCasesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllCompradorUseCase, GetAllCompradorUseCase>();
        services.AddScoped<IGetByIdCompradorUseCase, GetByIdCompradorUseCase>();
        services.AddScoped<IPutCompradorUseCase, PutCompradorUseCase>();
        services.AddScoped<IPostCompradorUseCase, PostCompradorUseCase>();
        services.AddScoped<IDeleteByIdCompradorUseCase, DeleteByIdCompradorUseCase>();
        services.AddScoped<IPatchCompradorUseCase, PatchCompradorUseCase>();
    }
}
