namespace PRN231_SU25_SE173175.Repository.Base
{
	public class BasePaginatedList<T>
	{
		public List<T> Items { get; set; }
		public int TotalItems { get; private set; }
		public int? CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int? PageSize { get; private set; }

		public BasePaginatedList() { }
		public BasePaginatedList(List<T> items, int count, int? pageNumber, int? pageSize)
		{
			TotalItems = count;
			CurrentPage = (pageNumber > 0 ? pageNumber : 1) ?? 1;
			PageSize = (pageSize > 0 ? pageSize : 6) ?? 6;
			TotalPages = PageSize > 0 ? (int)Math.Ceiling(count / (double)PageSize) : 1;
			Items = items;
		}

		public bool HasPreviousPage => CurrentPage > 1;
		public bool HasNextPage => CurrentPage < TotalPages;

	}
}
