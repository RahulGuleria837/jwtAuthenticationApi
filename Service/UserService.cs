using JWTAuthentication.ServiceContract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var roleUser = await _userManager.GetRolesAsync(user);
            user.Role = roleUser.FirstOrDefault(); 
            var userVerification = await _signInManager.CheckPasswordSignInAsync(user, userPassword, false);

            //JWT/;

            
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddSeconds(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);

            /*user.Token = tokenHandler.WriteToken(token);*/
            

            var refreshToekn = tokenHandler.CreateToken(tokenDescritor);
            user.Token = tokenHandler.WriteToken(refreshToekn);

            if (user.RefreshTokenExpiry > DateTime.Now) return user;

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
               var c= Convert.ToBase64String(randomNumber);
                user.RefreshToken = c;

            }
            user.RefreshTokenExpiry = DateTime.Now.AddDays(1);
            /*var user = await _userManager.CreateAsync(userCredentials, userCredentials.PasswordHash)*/
            await _userManager.UpdateAsync(user);
            
            
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
        public async Task<ApplicationUser> AddUserRefreshToken(ApplicationUser user)
        {
            var userDetail = await _userManager.UpdateAsync(user);
            if (userDetail.Succeeded) return user;
            return null;
        }

        public async Task<bool> UpdateUserRefreshToken(ApplicationUser user)
        {
            var updateUser = await AddUserRefreshToken(user);
            if (updateUser == null) return false;
            return true;
        }
        public async Task<ApplicationUser> GetUserRefreshToken(ApplicationUser user)
        {
            var findUserDetail = await _userManager.FindByNameAsync(user.UserName);
            if (findUserDetail == null) return null;
            return findUserDetail;
        }
    }
}
    

