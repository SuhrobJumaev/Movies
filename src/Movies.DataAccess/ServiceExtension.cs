using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.DataAccess;


public static class ServiceExtension
{
    private static string DefaultConnectionKeyName => "DefaultConnection";
    private static string ConnectionStringsSectionName => "ConnectionStrings";

    public static void ConfigureDataAcces(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(configuration.GetSection(ConnectionStringsSectionName)[DefaultConnectionKeyName]!));

        services.AddSingleton<DbInitializer>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
    }
}