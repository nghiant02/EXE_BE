public class ProductFilterDTO
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public List<string> Colors { get; set; } = new List<string>();
    public List<string> Sizes { get; set; } = new List<string>();
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public string? SortBy { get; set; } 
    public bool Sort { get; set; } // true for descending, false for ascending
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
