using JWTAuthentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Repository
{
  public  interface IJWTRepository
    {
        ApplicationUser GenerateToken(ApplicationUser user);
        ApplicationUser GenerateRefreshToken(ApplicationUser user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
