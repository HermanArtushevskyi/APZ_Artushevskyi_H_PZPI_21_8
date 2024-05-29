using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class Balance
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public float Amount { get; set; }
}