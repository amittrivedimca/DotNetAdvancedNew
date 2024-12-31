using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AuthenticationAPI.Classes
{
    public class UserLogin
    {
        List<AppUser> UserLogins;
        AppRole manager;
        AppRole customer;
        public UserLogin()
        {
            PrepareData();
        }

        private void PrepareData()
        {
            manager = new AppRole()
            {
                Name = "Manager",
                Permissions = new RolePermissions
                {
                    Create = true,
                    Delete = true,
                    Read = true,
                    Update = true
                }
            };
            customer = new AppRole()
            {
                Name = "Store customer",
                Permissions = new RolePermissions
                {
                    Create = false,
                    Delete = false,
                    Read = true,
                    Update = false
                }
            };

            UserLogins = new List<AppUser>() {
             new AppUser(){ UserName = "manager", Password = "password", Role = manager },
             new AppUser(){ UserName = "customer", Password = "password", Role = customer },
            };
        }

        public async Task<(bool,string)> DoCookieLogin(UserLoginModel userLogin, HttpContext ctx)
        {
            AppUser? appUser = UserLogins.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);
            if(appUser != null)
            {
                List<Claim> claims = new List<Claim>(); // e.g Licence properties
                claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                claims.Add(new Claim(ClaimTypes.Role, appUser.Role.Name));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // e.g. License                
                var user = new ClaimsPrincipal(identity);
                await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                return (true, "Success");
            }

            return (false, "Invalid username or password");
        }

        public string GetLoggedInUser(HttpContext ctx)
        {
            if (ctx.User.Identity.IsAuthenticated)
            {
                return ctx.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            }
            return "User is not logged in !!";
        }
    }
}
