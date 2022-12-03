using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Método de extensão que adiciona os mapeamentos do AutoMapper
    /// </summary>
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(Program).Assembly);
    }

    /// <summary>
    /// Método de extensão que configura o database e adiciona o DbContext
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    /// <summary>
    /// Método de extensão que configura o Identity
    /// </summary>
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    /// <summary>
    /// Método de extensão adiciona os services
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<PredictionService>();
        services.AddScoped<MatchService>();
        services.AddScoped<TeamService>();
        services.AddScoped<PointsService>();
        services.AddScoped<ResultPointsService>();

        return services;
    }
}
