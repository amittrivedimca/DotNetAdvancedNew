using AuthenticationAPI.Classes;
using AuthenticationAPI.Models;
using CommonUtils;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserLogin _userLogin;
        IHttpContextAccessor _contextAccessor;
        public UsersController(UserLogin userLogin,ILogger<UsersController> logger, IHttpContextAccessor contextAccessor)
        {
            _userLogin = userLogin;
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
            var result = await this._userLogin.GetJWT(loginModel, _contextAccessor.HttpContext);
            return Ok(new
            {
                IsSuccess = result.isSuccess,
                AccessToken = result.accessToken,
                RefreshToken = result.refreshToken.Token
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
            UserInfoModel result = await _userLogin.VerifyJWT(token);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<NewAccessTokenModel>> GetNewAccessToken(string refreshToken)
        {
            var result = await this._userLogin.GetNewAccessToken(refreshToken);
            NewAccessTokenModel res = new NewAccessTokenModel() {            
                IsSuccess = result.isSuccess,
                NewAccessToken = result.accessToken,
                RefreshToken = result.refreshToken            
            };
            return Ok(res);
        }
    }


}
