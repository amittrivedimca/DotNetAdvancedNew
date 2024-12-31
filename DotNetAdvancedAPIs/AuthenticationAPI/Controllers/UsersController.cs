using AuthenticationAPI.Classes;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    
    }


}
