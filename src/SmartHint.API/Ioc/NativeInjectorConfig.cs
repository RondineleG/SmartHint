namespace SmartHint.API.Ioc;

public static class NativeInjectorConfig
{
    public static void RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerAndConfigApiVersioning();
        services.AddAndConfigSwagger();
        var connection = configuration["DefaultConnection:ConnectionString"];
        services.AddDbContext<EntityFrameworkDataContext>(options => options.UseSqlite(connection));
        services.RegisterLogServices();
        services.RegisterUseCasesServices();
        services.RegisterServicesAndRepositoriesServices();
        services.RegisterLibrariesServices();
    }

    public static void UseApplicationServices(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwaggerUI();
        }

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.MapControllers();
    }
}
