using SmartHint.Application.Services;
using SmartHint.Core.Interfaces.Repositories;

namespace SmartHint.API.Extensions;

public static class ServicesAndRepositoriesServicesExtensions
{
    public static void RegisterServicesAndRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped<ICompradorRepositorio, CompradorRepositorio>();

        services.AddScoped<ICompradorServico, CompradorServico>();
        services.AddScoped<IValidacaoService, ValidacaoService>();
    }
}
