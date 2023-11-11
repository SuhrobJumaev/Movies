
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public static class ServiceExtension
{
    public static void ConfigureMoviesServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDataAcces(configuration);

        services.AddSingleton<IUserService, UserService>();
    }   
}

