namespace Tutorial9_EFCore_DBFirst.Exceptions;

public class PageNotFoundException(int page, int pageSize, int allPages) : 
    BaseExceptions.NotFoundException($"Page {page} exceeds maximum page number {allPages} with page size {pageSize}");