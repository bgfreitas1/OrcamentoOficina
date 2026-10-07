using OrcamentoOficina.Application;
using OrcamentoOficina.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Infrastructure.Persistence;
using OrcamentoOficina.Infrastructure.Persistence.Seed;
using OrcamentoOficina.API.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>(name: "database");

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();

    var dbContext =scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    await DatabaseSeeder.SeedAsync(dbContext);

    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Orçamento Oficina API v1");

        options.RoutePrefix = "swagger";

        options.DocumentTitle = "Orçamento Oficina API";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}