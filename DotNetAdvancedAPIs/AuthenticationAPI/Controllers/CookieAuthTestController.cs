using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    //public class CookieAuthTestController : ControllerBase
    //{

    //    #region "Policy based authorization"
    //    [Authorize(policy: "customer-policy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //    [HttpGet]
    //    public ActionResult SecureCustomer()
    //    {
    //        return Ok("success");
    //    }

    //    [Authorize(policy: "manager-policy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //    [HttpGet]
    //    public ActionResult SecureManager()
    //    {
    //        return Ok("success");
    //    }
    //    #endregion

    //    #region "Role based authorization"
    //    [Authorize( Roles = "StoreCustomer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //    [HttpGet]
    //    public ActionResult SecureCustomerRole()
    //    {
    //        return Ok("success");
    //    }

    //    [Authorize(Roles = "Manager", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //    [HttpGet]
    //    public ActionResult SecureManagerRole()
    //    {
    //        return Ok("success");
    //    }
    //    #endregion

    //    [HttpGet]
    //    public ActionResult NotSecure()
    //    {
    //        return Ok("success");
    //    }
    //}
}
