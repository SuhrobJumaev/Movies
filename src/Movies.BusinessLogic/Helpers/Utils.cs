using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public static class Utils
{
    public static string ConnectionStringsSectionName => "ConnectionStrings";
    public static string JwtSettingsSectionName => "JwtSettings";
    public static string JsonContentType => "application/json";

    public const string UserRole = "User";
    public const string AdminRole = "Admin";
}
