using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace webapidemo.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetUserId(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                return identity.FindFirst("userid").Value;
            }

            throw new System.Exception("User identity not found");
        }
    }
}