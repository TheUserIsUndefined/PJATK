namespace Tutorial9_EFCore_DBFirst.DTOs.Responses;

public class PaginatedResult<T> 
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public IEnumerable<T> Data { get; set; }
}