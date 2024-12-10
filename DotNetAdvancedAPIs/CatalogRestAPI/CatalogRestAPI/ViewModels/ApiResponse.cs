namespace CatalogRestAPI.ViewModels
{
    public class ApiResponse<T> where T : class
    {
        public T Data { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
