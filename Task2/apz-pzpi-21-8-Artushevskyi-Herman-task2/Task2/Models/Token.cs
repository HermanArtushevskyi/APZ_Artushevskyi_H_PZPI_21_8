using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class Token
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Value { get; set; }
    [Required]
    public int UserId { get; set; }
}