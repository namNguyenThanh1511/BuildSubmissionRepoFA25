namespace PRN231_SU25_SE184386.DAL.ModelExtensions
{
    public record PagingResponse<T>(IEnumerable<T>? Items, int TotalCount, int PageNumber, int PageSize) where T : class;  
}
