using JWTAuthentication.ServiceContract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Service
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationSigninManager _signInManager;
        private readonly ApplicationUserManager _userManager;


        public UserService(ApplicationSigninManager signInManager, ApplicationUserManager userManager, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> AuthenticateUser(string userName, string userPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userVerification = await _signInManager.CheckPasswordSignInAsync(user, userPassword, false);

            //JWT/
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            user.RefreshToken = tokenHandler.WriteToken(token);
            user.PasswordHash = "";
           if(!userVerification.Succeeded) return null;
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
    

