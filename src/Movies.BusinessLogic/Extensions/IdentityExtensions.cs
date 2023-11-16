﻿
using Microsoft.AspNetCore.Http;

namespace Movies.BusinessLogic;

public static class IdentityExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == "UserId");

        return int.Parse(userId?.Value!);
    }

    public static string GetUserRole(this HttpContext context)
    {
        var Role = context.User.Claims.SingleOrDefault(x => x.Type == "Role");

        return Role?.Value!;
    }
}
