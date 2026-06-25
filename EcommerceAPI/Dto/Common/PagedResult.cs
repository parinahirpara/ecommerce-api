namespace EcommerceAPI.Dto.Common
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int CurrentCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public PagedResult() { }

        public PagedResult(List<T> items, int totalItems, int skip)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentCount = skip + items.Count;
        }
    }
}
