using AuthenticationAPI.Classes;

namespace AuthenticationAPI.Models
{
    public class UserInfoModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public AppRole Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}
