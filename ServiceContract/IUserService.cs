using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.ServiceContract
{
   public interface IUserService
    {
        Task<bool> IsUnique(string userName);
        Task<ApplicationUser> AuthenticateUser(string userName, string userPassword);
        Task<bool> RegisterUser(ApplicationUser userCredentials);
    }
}
