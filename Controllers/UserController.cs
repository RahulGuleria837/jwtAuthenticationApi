using AutoMapper;
using JWTAuthentication.Model;
using JWTAuthentication.Model.DTO;
using JWTAuthentication.Repository;
using JWTAuthentication.ServiceContract;
using JWTAuthentication.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationUserManager _userManager;
        private readonly IJWTRepository _jwtManager;
        private readonly IMapper _mapper;


        public UserController(IUserService userService,  ApplicationUserManager userManager, IJWTRepository jwtManager, IMapper mapper )
        {
            _userService = userService;
            _jwtManager = jwtManager;
            _userManager = userManager;
            _mapper = mapper;

        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (await _userService.IsUnique(login.UserName))
                return Ok(new { Message = "Please Register first then login!!!" });
            var userAuthorize = await _userService.AuthenticateUser(login.UserName, login.Password);
            if (userAuthorize == null) return Unauthorized();
            return Ok(new { accesstoken = userAuthorize.Token ,userAuthorize.RefreshToken });
        }
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDetail)
        {

            var ApplicationUserDetail = _mapper.Map<ApplicationUser>(userRegisterDetail);
            ApplicationUserDetail.PasswordHash = userRegisterDetail.Role;
            if (ApplicationUserDetail == null || !ModelState.IsValid) return BadRequest();
            if (!await _userService.IsUnique(userRegisterDetail.UserName)) return Ok(new { Message = "You are already register go to login" });
            var registerUser = await _userService.RegisterUser(ApplicationUserDetail);
            if (!registerUser) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(new { Message = "Register successfully!!!" });

        }
        [Route("RefreshToken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken(Tokens userToken)
        {
            Request.Headers.TryGetValue("IS-TOKEN-EXPIRED", out var headerValue);
            if (userToken == null || !ModelState.IsValid || headerValue.FirstOrDefault() == "")
            {
                return BadRequest();
            } 
            var claimUserDataFromToken = _jwtManager.GetPrincipalFromExpiredToken(userToken.AccessToken);
            if (claimUserDataFromToken == null)
            {
                return Unauthorized(new { Message = "your token is valid sir use this token" });
            }
            var claimUserIdentity = (ClaimsIdentity)claimUserDataFromToken.Identity;
            var claimUser = claimUserIdentity.FindFirst(ClaimTypes.Name);
            if (claimUser == null)
            {
                return Unauthorized();
            }
            

            var checkUserInDb = await _userManager.FindByIdAsync(claimUser.Value);
            if (checkUserInDb == null) return Unauthorized();
            var userGetRole = await _userManager.GetRolesAsync(checkUserInDb);
            checkUserInDb.Role = userGetRole.FirstOrDefault();
            if (checkUserInDb.RefreshToken != userToken.RefreshToken)
            {
                return Unauthorized(new { Message = "Go to login!!!!!!" });
            }
            var generateNewToken = _jwtManager.GenerateToken(checkUserInDb);
            if (!await _userService.UpdateUserRefreshToken(generateNewToken))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Tokens usertoken = new Tokens()
            {
                AccessToken = generateNewToken.Token,
                RefreshToken = generateNewToken.RefreshToken,
            };
            return Ok(usertoken);
        }

    }
}

