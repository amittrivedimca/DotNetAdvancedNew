using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace AuthenticationAPI.Classes
{
    public class UserLogin
    {
        List<AppUser> UserLogins;
        AppRole manager;
        AppRole customer;
        public const string secret = "fjDFFf8wur482r842r902dcFDkfvcmNGdc909323";

        public static TokenValidationParameters TokenValidationParams = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
        };

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
                Name = "StoreCustomer",
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

        /// <summary>
        /// Generate cookie if user is authenticated. 
        /// Cookie is used in sub-sequent requests to determine user is logged in or not
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
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

        public async Task<(bool, string)> GetJWT(UserLoginModel userLogin, HttpContext ctx)
        {
            var handler = new JsonWebTokenHandler();

            AppUser? appUser = UserLogins.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);

            if (appUser != null)
            {
                //var rsaKey = RSA.Create();
                //var key = new RsaSecurityKey(rsaKey);
                
                List<Claim> claims = new List<Claim>(); // e.g Licence properties
                claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                claims.Add(new Claim(ClaimTypes.Role, appUser.Role.Name));
                var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme); // e.g. License

                var tokenDesc = new SecurityTokenDescriptor()
                {
                    Issuer = "TestIssuer",
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    Subject = identity,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                string token = handler.CreateToken(tokenDesc);

                //var user = new ClaimsPrincipal(identity);
                //await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                return (true,token);
            }

            return (false, "Invalid username or password");
        }

        public async Task<TokenValidationResult> VerifyJWT(string token)
        {
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();            
            TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(token, TokenValidationParams);
            return validationResult;            
        }
    }
}
