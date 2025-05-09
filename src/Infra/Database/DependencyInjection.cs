﻿using Infra.Database.DbContexts;
using Infra.Database.Helpers;
using Infra.Database.Repositories;
using Infra.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }

    public static void SeedDatabase(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        context.Database.Migrate();
        
        SeedData.Seed(context);
    }

    public static IServiceCollection AddInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IAssuntoRepository, AssuntoRepository>();
        services.AddScoped<IAutorRepository, AutorRepository>();
        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<ILivroAssuntoRepository, LivroAssuntoRepository>();
        services.AddScoped<ILivroAutorRepository, LivroAutorRepository>();
        services.AddScoped<ILivroPrecoRepository, LivroPrecoRepository>();

        return services;
    }
}
