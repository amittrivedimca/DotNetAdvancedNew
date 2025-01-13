using AuthenticationAPI.Models;
using CommonUtils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AuthenticationAPI.Classes
{
    public class UserLogin
    {
        List<AppUser> UserLoginDB;        
        AppRole manager;
        AppRole customer;
        public const string secret = "fjDFFf8wur482r842r902dcFDkfvcmNGdc909323";
        LoggedInUsersCollection _loggedInUsers;

        public static readonly TokenValidationParameters TokenValidationParams = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };

        public UserLogin(LoggedInUsersCollection loggedInUsers)
        {
            _loggedInUsers = loggedInUsers;
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

            UserLoginDB = new List<AppUser>() {
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
            AppUser? appUser = UserLoginDB.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);
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

        public (string userName, ClaimsPrincipal user) GetLoggedInUser(HttpContext ctx)
        {
            if (ctx.User.Identity.IsAuthenticated)
            {
                string name = ctx.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                return (name, ctx.User);
            }
            return ("User is not logged in !!", null);
        }

        public async Task<(bool isSuccess, string accessToken, RefreshToken refreshToken)> GetJWT(UserLoginModel userLogin, HttpContext ctx)
        {
                     
            AppUser? appUser = UserLoginDB.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);

            if (appUser != null)
            {
                //var rsaKey = RSA.Create();
                //var key = new RsaSecurityKey(rsaKey);

                string token = GenerateJWT(appUser.UserName, appUser.Role.Name);
                RefreshToken refreshToken = GenerateRefreshToken();

                _loggedInUsers.Add(new LoggedInUser() { JWTToken = token, RefreshToken = refreshToken, 
                     UserName = appUser.UserName ,RoleName = appUser.Role.Name
                });

                return (true,token, refreshToken);
            }

            return (false, "Invalid username or password",new RefreshToken());
        }

        private string GenerateJWT(string UserName,string RoleName) {
            var handler = new JsonWebTokenHandler();
            List<Claim> claims = new List<Claim>(); // e.g Licence properties
            claims.Add(new Claim(ClaimTypes.Name, UserName));
            claims.Add(new Claim(ClaimTypes.Role, RoleName));
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
            return token;
        }

        public async Task<UserInfoModel> VerifyJWT(string token)
        {                        
            TokenValidationResult validationResult = await VerifyJWTToken(token);
            
            UserInfoModel userInfo = new UserInfoModel() { 
             IsAuthenticated = validationResult.IsValid,
              ErrorMessage = validationResult.Exception?.Message              
            };
            bool isTokenExpired = false;

            if (!validationResult.IsValid)
            {
                isTokenExpired = validationResult.Exception is SecurityTokenExpiredException;
                userInfo.IsTokenExpired = validationResult.Exception is SecurityTokenExpiredException;
            }

            if (isTokenExpired || validationResult.IsValid)
            {
                var loggedInUser = _loggedInUsers.FindByJWT(token);
                if (loggedInUser != null)
                {
                    string userName = loggedInUser.UserName; //validationResult.ClaimsIdentity.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                    AppUser appUser = UserLoginDB.First(u => u.UserName == userName);
                    userInfo.UserName = appUser.UserName;
                    userInfo.Role = appUser.Role;
                    userInfo.RefreshToken = loggedInUser.RefreshToken.Token;
                }
            }            

            return userInfo;   
        }

        private async Task<TokenValidationResult> VerifyJWTToken(string token)
        {
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
            TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(token, TokenValidationParams);
            return validationResult;
        }

        public async Task<NewAccessTokenModel> GetNewAccessToken(string refreshToken)
        {

            var loggedInUser = _loggedInUsers.FindByRefreshToken(refreshToken);

            if (loggedInUser == null)
            {
                return new NewAccessTokenModel()
                {

                    IsSuccess = false,
                    ErrorMessage = "Invalid token !"
                };
            }

            if (loggedInUser.RefreshToken.IsExpired)
            {
                return new NewAccessTokenModel()
                {

                    IsSuccess = false,
                    ErrorMessage = "Refresh token is expired! Please log in again!"
                };
            }

            TokenValidationResult validationResult = await VerifyJWTToken(loggedInUser.JWTToken);

            if (validationResult.IsValid)
            {
                return new NewAccessTokenModel()
                {

                    IsSuccess = false,
                    ErrorMessage = "Existing token is not expired yet!"
                };
            }

            bool isTokenExpired = validationResult.Exception is SecurityTokenExpiredException;

            if (isTokenExpired && loggedInUser != null && loggedInUser.RefreshToken.IsExpired == false)
            {

                loggedInUser.JWTToken = GenerateJWT(loggedInUser.UserName, loggedInUser.RoleName);
                //loggedInUser.RefreshToken = GenerateRefreshToken();                
                //return (true, loggedInUser.JWTToken, loggedInUser.RefreshToken.Token);
                return new NewAccessTokenModel()
                {
                    IsSuccess = true,
                    NewAccessToken = loggedInUser.JWTToken,
                    RefreshToken = loggedInUser.RefreshToken.Token
                };
            }

            //return (false, "Something went wrong !", "");
            return new NewAccessTokenModel()
            {
                IsSuccess = false,
                ErrorMessage = "Something went wrong !"
            };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var rn = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(rn);
                string token = HttpUtility.UrlEncode(Convert.ToBase64String(rn));
                return new RefreshToken()
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }
    }
}
