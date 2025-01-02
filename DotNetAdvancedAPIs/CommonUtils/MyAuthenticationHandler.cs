using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace CommonUtils
{

    public class MyAuthenticationOptions : AuthenticationSchemeOptions //RemoteAuthenticationOptions
    {        
    }

    public class MyAuthenticationHandler :  AuthenticationHandler<MyAuthenticationOptions> //RemoteAuthenticationHandler<MyAuthenticationOptions>
    {
        // Static HttpClient to be reused throughout the lifetime of the application.
        private static readonly HttpClient httpClient = new HttpClient();
        MyAuthenticationOptions _options;
        UserService _userService;
        private string MyAuthUrl = "http://localhost:5046/api/users";
        ILogger _logger;

        public MyAuthenticationHandler(UserService userService, IOptionsMonitor<MyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options,logger,encoder)
        {
            _userService = userService;
            _options = options.CurrentValue;
            _logger = logger.CreateLogger<MyAuthenticationHandler>();
        }

        //protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Read the token from request headers/cookies
            string authHeaderStr = null;
            string token = null;
            if (Request != null && Request.Headers.ContainsKey("Authorization"))
            {                
                authHeaderStr = Request.Headers["Authorization"].ToString();

                _logger.LogTrace(">>>>>>>>>>>> Authorization Token: " + authHeaderStr);

                token = authHeaderStr.Replace("Bearer ", "");
                (bool isValid, AuthenticationTicket ticket) result = await VerifyToken(token);

                if (result.isValid)
                {
                    // If the token is valid, return success:           
                    return HandleRequestResult.Success(result.ticket);
                }
            }

            // If the token is missing or the session is invalid, return failure:
            return HandleRequestResult.Fail("Authentication failed");
        }

        private async Task<(bool isValid, AuthenticationTicket ticket)> VerifyToken(string token)
        {
            string url = MyAuthUrl + "/VerifyJWT?token=" + token;
            var request = new HttpRequestMessage(HttpMethod.Post, url); // Prepare the HTTP request.                                                                       
            var response = await httpClient.SendAsync(request); // Send the request.
            return await ProcessResponse(response);
        }

        private async Task<(bool isValid, AuthenticationTicket ticket)> ProcessResponse(HttpResponseMessage responseMessage)
        {
            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            UserInfoModel userInfo = JsonSerializer.Deserialize<UserInfoModel>(jsonString);

            if (userInfo.IsAuthenticated)
            {
                _userService.CurrentUser = userInfo;
                var claims = new[] { 
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim(ClaimTypes.Role, userInfo.Role.Name)
                };
                var identity = new ClaimsIdentity(claims, "Tokens");
                var principal = new ClaimsPrincipal(identity);  
                var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
                return (true, ticket);
            }
            else
            {
                _userService.CurrentUser = null;
                return (false, null);
            }
        }
       
        
    }
}
