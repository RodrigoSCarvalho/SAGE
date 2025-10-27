using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SAGE.Application.ChangePlans.Commands.CreateChangePlan;
using SAGE.Application.ChangePlans.Mappings;
using SAGE.Application.DependencyInjection;
using SAGE.Domain.ChangePlans;
using SAGE.Infrastructure.DependencyInjection;
using SAGE.Infrastructure.Persistence.Context;
using SAGE.Infrastructure.Persistence.Migrations;
using SAGE.Infrastructure.Persistence.Repositories;
using SAGE.Infrastructure.Search;

var builder = WebApplication.CreateBuilder(args);

var dbHost = builder.Configuration["DB_HOST"] ?? "localhost";
var dbPort = builder.Configuration["DB_PORT"] ?? "5432";
var dbName = builder.Configuration["DB_NAME"] ?? "sage_db";
var dbUser = builder.Configuration["DB_USER"];
var dbPassword = builder.Configuration["DB_PASSWORD"];

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SAGE API",
        Version = "v1",
        Description = "API para Gestão de Mudanças"
    });
    c.EnableAnnotations();
});

builder.Services.AddAutoMapper(typeof(ChangePlanProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateChangePlanValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(CreateChangePlan).Assembly).For.Migrations());


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
    .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<IChangePlanRepository, ChangePlanRepository>();


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            if (runner.HasMigrationsToApplyUp())
            {
                Console.WriteLine("Executando migrations");
                runner.MigrateUp();
                Console.WriteLine("Migrations executadas com sucesso.");
            }
            else
            {
                Console.WriteLine("Banco de dados já atualizado.");
            }

            var elasticsearchInitializer = scope.ServiceProvider.GetRequiredService<ElasticsearchInitializer>();
            Console.WriteLine("Inicializando Elasticsearch...");
            await elasticsearchInitializer.InitializeAsync();
            Console.WriteLine("Elasticsearch inicializado com sucesso.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while applying database migrations: {ex.Message}");
            throw;
        }
    }
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
     {
         context.Response.StatusCode = 500;
         context.Response.ContentType = "application/json";
         var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
         if (error != null)
         {
             var ex = error.Error;
             Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
             await context.Response.WriteAsJsonAsync(new
             { error = app.Environment.IsDevelopment() ? ex.Message : "Ocorreu um erro interno no servidor." });
         }
     });
});
app.UseHttpsRedirection();
app.UseCors("AllowFrontEnd");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAGE API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();
Console.WriteLine($"SAGE API rodando em: {app.Environment.EnvironmentName}");
Console.WriteLine($"Swagger disponível em: http://localhost:5093");
app.Run();