public class ProductFilterDTO
{
    public string? Search { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public string? SortBy { get; set; } 
    public bool Sort { get; set; } // true for descending, false for ascending
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
