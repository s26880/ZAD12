using System.ComponentModel.DataAnnotations;

namespace CW_10_s30071.Models.DTOs;

public class AddClientToTripDTO
{
    [Length(2,20)]
    [Required]
    public string FirstName { get; set; }
    [Length(2,20)]
    [Required]
    public string LastName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Phone]
    [Required]
    public string Telephone { get; set; }
    [Length(11,11)]
    [Required]
    public string Pesel { get; set; }
    [Required]
    public DateTime? PaymentDate { get; set; }
}
