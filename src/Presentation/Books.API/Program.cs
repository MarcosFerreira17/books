using Application;
using Infra.Database;
using QuestPDF.Infrastructure;
using Books.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddSwaggerExtensions();

builder.Services.AddApiVersioningExtensions();

builder.Services.AddProblemDetailsExtensions();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");

builder.Services.AddDatabase(connectionString);

builder.Services.AddInfraServices();

builder.Services.AddApplicationServices();

builder.Services.AddCorsExtensions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TJRJ v1");
    });
    
    app.SeedDatabase();
}

app.UseCors("CorsPolicy");

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

