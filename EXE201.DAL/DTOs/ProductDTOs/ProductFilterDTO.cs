public class ProductFilterDTO
{
    public string? Search { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
