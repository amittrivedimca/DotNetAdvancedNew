namespace AuthenticationAPI.Classes
{
    public class RolePermissions
    {
        public bool Create { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool Read { get; set; } = false;
    }
}
