using JWTAuthentication.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationSigninManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        public UserService(ApplicationSigninManager signInManager, ApplicationUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> AuthenticateUser(string userName, string userPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userVerification = await _signInManager.CheckPasswordSignInAsync(user, userPassword, false);
            if (!userVerification.Succeeded) return null;
            return user;
        }

        public async Task<bool> IsUnique(string userName)
        {
            var userExist = await _userManager.FindByNameAsync(userName);
            if (userExist == null) return true;
            return false;
        }

        public async Task<bool> RegisterUser(ApplicationUser userCredentials)
        {
            var user = await _userManager.CreateAsync(userCredentials, userCredentials.PasswordHash);
            if (!user.Succeeded) return false;
            await _userManager.AddToRoleAsync(userCredentials, userCredentials.Role);
            return true;
        }
    }
}
    

