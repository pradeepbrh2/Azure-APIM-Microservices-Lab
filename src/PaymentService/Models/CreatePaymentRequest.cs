
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models;

public class CreatePaymentRequest
{
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(typeof(decimal), "0.01", "10000000")]
    public decimal Amount { get; set; }

    [Required]
    [RegularExpression("^(UPI|Card|NetBanking)$")]
    public string PaymentMethod { get; set; } = string.Empty;
}
