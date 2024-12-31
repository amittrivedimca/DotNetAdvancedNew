namespace AuthenticationAPI.Classes
{
    public class AppUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public AppRole Role { get; set; }
    }
}
