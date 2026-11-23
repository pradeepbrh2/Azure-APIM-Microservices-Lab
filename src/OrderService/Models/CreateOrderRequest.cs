
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class CreateOrderRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal UnitPrice { get; set; }
}
