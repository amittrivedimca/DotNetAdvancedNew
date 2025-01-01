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

        //[HttpPost]
        //public async Task<ActionResult> DoCookieLogin(UserLoginModel userLogin)
        //{
        //    var result = await this.userLogin.DoCookieLogin(userLogin, _contextAccessor.HttpContext);                        
        //    return Ok(result.Item2);            
        //}

        //[HttpGet]
        //public ActionResult GetCookieLoggedInUser()
        //{
        //    var userInfo = userLogin.GetLoggedInUser(_contextAccessor.HttpContext);
        //    return Ok(new
        //    {
        //        userName = userInfo.userName,
        //        user = userInfo.user
        //    });
        //}

        /// <summary>
        /// Generate JWT
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> JWT(UserLoginModel loginModel)
        {
            var result = await this.userLogin.GetJWT(loginModel, _contextAccessor.HttpContext);
            return Ok(new
            {
                IsSuccess = result.isSuccess,
                AccessToken = result.accessToken
            });
        }

        /// <summary>
        /// Verify JWT and return user role with permissions if token is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns>User role and permissions if token is valid</returns>
        [HttpPost]        
        public async Task<ActionResult<UserInfoModel>> VerifyJWT(string token)
        {
            UserInfoModel result = await userLogin.VerifyJWT(token);            
            return Ok(result);
        }
    }


}
