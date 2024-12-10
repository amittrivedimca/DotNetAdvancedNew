namespace HelperUtils
{
    public abstract class PageFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Skip
        {
            get
            {
                return (PageNumber - 1) * PageSize;
            }
        }

        public bool IsValidPageFilter()
        {
            return PageNumber > 0 && PageSize > 0;
        }
    }
}
