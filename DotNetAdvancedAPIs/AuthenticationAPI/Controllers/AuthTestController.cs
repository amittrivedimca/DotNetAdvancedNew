using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public ActionResult Secure()
        {
            return Ok("success");
        }

        
        [HttpGet]
        public ActionResult NotSecure()
        {
            return Ok("success");
        }
    }
}
