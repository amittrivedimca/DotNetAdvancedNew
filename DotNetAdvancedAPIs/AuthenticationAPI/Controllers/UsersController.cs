using AuthenticationAPI.Classes;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserLogin userLogin;
        IHttpContextAccessor _contextAccessor;
        public UsersController(ILogger<UsersController> logger, IHttpContextAccessor contextAccessor)
        {
            userLogin = new UserLogin();
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult> DoCookieLogin(UserLoginModel userLogin)
        {
            var result = await this.userLogin.DoCookieLogin(userLogin, _contextAccessor.HttpContext);                        
            return Ok(result.Item2);            
        }

        [HttpGet]
        public ActionResult GetLoggedInUser()
        {
            var userName = userLogin.GetLoggedInUser(_contextAccessor.HttpContext);
            return Ok(new { userName = userName });
        }

        [HttpPost]
        public async Task<ActionResult> JWT(UserLoginModel loginModel)
        {
            var result = await this.userLogin.GetJWT(loginModel, _contextAccessor.HttpContext);
            return Ok(new
            {
                IsSuccess = result.Item1,
                Token = result.Item2
            });
        }

        [HttpPost]
        public async Task<ActionResult> VerifyJWT(string token)
        {
            var result = await userLogin.VerifyJWT(token);
            return Ok(new
            {
                IsValid = result.IsValid,
                Message = result.Exception?.Message
            });
        }
    }


}
