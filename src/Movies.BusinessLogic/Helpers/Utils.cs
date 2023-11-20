
namespace Movies.BusinessLogic;

public static class Utils
{
    public static string ConnectionStringsSectionName => "ConnectionStrings";
    public static string JwtSettingsSectionName => "JwtSettings";
    public static string JwtSettingsKeyColumnName => "JwtSettings:Key";
    public static string JwtSettingsIssuerColumnName => "JwtSettings:Issuer";
    public static string JwtSettingsAudienceColumnName => "JwtSettings:Audience";


    public const string JsonContentType = "application/json";
    public const string ApiVersionTag = "api-version";

    public const string UserRole = "User";
    public const string AdminRole = "Admin";

    public const int UserRoleByDefault = 2;

    public const string API_VERSION_1 = "1.0";
    public const string API_BERSION_2 = "2.0";

    public static string HealthCheckName => "DataBase";
    public static string HealthCheckErrorMessage => "DataBase is unhealthy";

    

    public static class ApiEndpoints
    {
        public const string HealthCheckEndpoint = "_health";
    }
}
