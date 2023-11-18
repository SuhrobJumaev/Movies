
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Movies.BusinessLogic;

public static class IdentityExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == "UserId");

        return int.Parse(userId?.Value);
    }

    public static string GetUserRole(this HttpContext context)
    {
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        return role!;
    }

    public static string GetUserEmail(this HttpContext context)
    {
        var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

        return email!;
    }
}
