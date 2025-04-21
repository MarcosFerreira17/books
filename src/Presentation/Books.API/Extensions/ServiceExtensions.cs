using Asp.Versioning;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;

namespace Books.API.Extensions;

public static class ServiceExtensions
{
    public static void AddCorsExtensions(this IServiceCollection services, string[] urls)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
            builder =>
            {
                builder.WithOrigins(urls)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public static void AddSwaggerExtensions(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Books API",
                Version = "v1",
                Description = "An API to perform Books operations",
                Contact = new OpenApiContact
                {
                    Name = "Marcos Ferreira",
                    Email = "marcosfw7@@outlook.com",
                }
            });
        });
    }

    public static void AddApiVersioningExtensions(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        })
        .AddMvc() // This is needed for controllers
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddProblemDetailsExtensions(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

    }

}
