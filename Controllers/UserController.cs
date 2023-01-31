using AutoMapper;
using JWTAuthentication.Model.DTO;
using JWTAuthentication.ServiceContract;
using JWTAuthentication.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (await _userService.IsUnique(login.UserName)) return Ok(new { Message = "Please Register first then login!!!" });
            var userAuthorize = await _userService.AuthenticateUser(login.UserName, login.Password);
            if (userAuthorize == null) return Unauthorized();
            return Ok(new { Message = "login successfully" });
        }
        [HttpPost]
        /*[Route("Register")]*/
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDetail)
        {

            var ApplicationUserDetail = _mapper.Map<ApplicationUser>(userRegisterDetail);
            ApplicationUserDetail.PasswordHash = userRegisterDetail.Password;
            if (ApplicationUserDetail == null || !ModelState.IsValid) return BadRequest();
            if (!await _userService.IsUnique(userRegisterDetail.UserName)) return Ok(new { Message = "You are already register go to login" });
            var registerUser = await _userService.RegisterUser(ApplicationUserDetail);
            if (!registerUser) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(new { Message = "Register successfully!!!" });

        }

    }
}

