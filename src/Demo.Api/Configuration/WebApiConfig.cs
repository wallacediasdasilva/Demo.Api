using Demo.Api.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Configuration;

public static class WebApiConfig
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ResolveDependencies();

        services.AddIdentityConfiguration(configuration);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void UseServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public static void ConfigureDbContext(this IServiceCollection services, ConfigurationManager configuration)
        => services.AddDbContext<MeuDbContext>(options =>
                                                  options.UseSqlServer(configuration.GetConnectionString("ORM")));
    public static void ConfigureAutoMapper(this IServiceCollection services) => services.AddAutoMapper(typeof(Program));
}
