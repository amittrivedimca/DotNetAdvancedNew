namespace CatalogRestAPI.ViewModels
{
    public class AppRole
    {
        public string Name { get; set; }
        public RolePermissions Permissions { get; set; }
    }

    public class RolePermissions
    {
        public bool Create { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool Read { get; set; } = false;
    }
}
