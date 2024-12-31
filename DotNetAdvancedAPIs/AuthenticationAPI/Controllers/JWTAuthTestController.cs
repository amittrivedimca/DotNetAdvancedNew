using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JWTAuthTestController : ControllerBase
    {
        #region "Role based authorization"
        [Authorize(Roles = "StoreCustomer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult SecureCustomerRole()
        {
            return Ok("success");
        }

        [Authorize(Roles = "Manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult SecureManagerRole()
        {
            return Ok("success");
        }
        #endregion

        [HttpGet]
        public ActionResult NotSecure()
        {
            return Ok("success");
        }
    }
}
