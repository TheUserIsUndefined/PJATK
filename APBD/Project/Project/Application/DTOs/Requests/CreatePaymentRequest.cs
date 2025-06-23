using System.ComponentModel.DataAnnotations;

namespace Project.Application.DTOs.Requests;

public class CreatePaymentRequest
{
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}