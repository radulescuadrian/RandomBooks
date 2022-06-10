using System.ComponentModel.DataAnnotations.Schema;

namespace RandomBooks.Shared.DatabaseModels;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DatePlaced { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;
    public bool InDelivery { get; set; } = false;
    public string? Notes { get; set; }
    public int? Rating { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public List<OrderItem> Books { get; set; }
    public Address Address { get; set; }
    public CustomerCard? Payment { get; set; }
    public int? EmployeeId { get; set; }
    public int? CourierId { get; set; }
    public int StatusId { get; set; } = 1;
}
