using Carter;
using Clean.Solutions.Vertical.Abstractions;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Pipeline;
using Clean.Solutions.Vertical.Shared;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Clean.Solutions.Vertical;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();


        builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

        var assembly = typeof(Program).Assembly;
        builder.Services.AddMediatR(o =>
        {
            o.RegisterServicesFromAssembly(assembly);
            //o.AddBehavior(typeof(UnhandledExceptionPipeline<,,>));
            o.AddOpenBehavior(typeof(ValidationPipeline<,>));
            o.AddOpenBehavior(typeof(LoggingPipeline<,>));
        });
        builder.Services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(UnhandledExceptionPipeline<,,>));
        builder.Services.AddValidatorsFromAssembly(assembly);
        builder.Services.AddCarter();
        builder.Services.AddFluentValidationAutoValidation(o =>
        {
            o.DisableDataAnnotationsValidation = true;
        })
        .AddFluentValidationClientsideAdapters();


        //builder.Services.Scan(
        //selector => selector
        //    .FromAssemblies(assembly)
        //    .AddClasses(false)
        //    .AsImplementedInterfaces()
        //    .WithScopedLifetime());

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();

        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        app.UseAuthorization();

        app.Run();
    }
}

