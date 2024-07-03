namespace EXE201.DAL.DTOs.RentalOrderDTOs;

public class ViewRentalOrderDetail
{
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public int? ProductQuantity { get; set; }
    public DateTime? RentalStart { get; set; }
    public DateTime? RentalEnd { get; set; }
    public string? PaymentType { get; set; }
    public string? Status { get; set; }

    public string? OrderCode { get; set; }

    //User
    public string? Username { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
}