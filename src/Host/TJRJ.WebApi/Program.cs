using Application;
using Infra.Database;
using Microsoft.AspNetCore.Http.Features;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using TJRJ.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddSwaggerExtensions();

builder.Services.AddApiVersioningExtensions();

builder.Services.AddProblemDetails(options =>
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

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");

builder.Services.AddDatabase(connectionString);

builder.Services.AddInfraServices();

builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TJRJ v1");
    });
}

app.SeedDatabase();

app.UseCors("CorsPolicy");

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

