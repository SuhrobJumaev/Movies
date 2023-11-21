
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
    public const string CreateRuleSetName = "Create";
    public const string EditRuleSetName = "Edit";
    public const string EditProfileRuleSetName = "EditProfile";

    public const string API_VERSION_1 = "1.0";
    public const string API_BERSION_2 = "2.0";

    public static string HealthCheckName => "DataBase";
    public static string HealthCheckErrorMessage => "DataBase is unhealthy";

    public const string PathToSaveFiles = "D:\\AlifAcademyC#\\Movies\\src\\Movies.Web\\wwwroot\\";
    public const string StreamVideoPath = "https:/localhost:7297/api/movies/stream-movie/";

    public const int ChunkSize = 1024;

    public static class ApiEndpoints
    {
        public const string HealthCheckEndpoint = "_health";
    }

    public static class ValidationErrorMessage
    {
        public const string MovieSortingErrorMessage = "Вы может сортировать только по колонке title и year.";
        public const string MoviePaginationErrorMessage = "Вы можете выбрать  от 10 до 50 записей на странице.";
        public const string EmailAlreadyExistsErrorMessage = "Пользователь с таким email'ом -{PropertyValue} уже существует.";
        public const string UserSortingErrorMessage = "Вы может сортировать только по колонке name и age";
        public const string UserPaginationErrorMessage = "Вы можете выбрать  от 10 до 50 записей на странице";
    }
}
