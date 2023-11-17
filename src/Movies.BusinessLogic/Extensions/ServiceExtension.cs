
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.DataAccess;
using System;

namespace Movies.BusinessLogic;

public static class ServiceExtension
{
    public static void ConfigureMoviesServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDataAcces(configuration);

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IMovieService, MovieService>();

        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();
    }   
}

