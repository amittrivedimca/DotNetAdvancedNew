using CommonUtils;
using System.Net;

namespace AuthenticationAPI.Classes
{
    public class LoggedInUser
    {
        public string JWTToken { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }

    public class LoggedInUsersCollection
    {
        private List<LoggedInUser> loggedInUsers { get; set; }
        public LoggedInUsersCollection()
        {
            loggedInUsers = new List<LoggedInUser>();
        }

        public void Add(LoggedInUser user)
        {
            var existingUser = loggedInUsers.FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser == null)
            {
                loggedInUsers.Add(user);
            }
            else
            {
                existingUser.JWTToken = user.JWTToken;
                existingUser.RefreshToken = user.RefreshToken;
                existingUser.RoleName = user.RoleName;
            }
        }

        public LoggedInUser FindByJWT(string jwtToken)
        {
            var user = loggedInUsers.FirstOrDefault(u => u.JWTToken == jwtToken);
            return user;
        }

        public LoggedInUser FindByRefreshToken(string refreshToken)
        {
            var decodedToken = WebUtility.UrlDecode(refreshToken);
            var user = loggedInUsers.FirstOrDefault(u => WebUtility.UrlDecode(u.RefreshToken.Token) == decodedToken);
            return user;
        }
    }
}
