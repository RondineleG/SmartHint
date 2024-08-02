﻿namespace SmartHint.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerAndConfigApiVersioning(
        this IServiceCollection services
    )
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";

                options.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();

        return services;
    }

    public static IServiceCollection AddAndConfigSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerOperationFilter>();
        });
        return services;
    }

    public static void UseCustomSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var apiVersionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
            if (apiVersionProvider == null)
                throw new ArgumentException("Versionamento de API não registrado.");

            for (var i = 0; i < apiVersionProvider.ApiVersionDescriptions.Count; i++)
            {
                var description = apiVersionProvider.ApiVersionDescriptions[i];
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName
                );
            }
            options.RoutePrefix = string.Empty;

            options.DocExpansion(DocExpansion.List);
        });
    }
}
