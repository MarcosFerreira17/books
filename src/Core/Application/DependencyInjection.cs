using Application.Services.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAssuntoService, AssuntoService>();
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<ILivroPrecoService, LivroPrecoService>();
        services.AddScoped<IRelatorioService, RelatorioService>();
    }
}
