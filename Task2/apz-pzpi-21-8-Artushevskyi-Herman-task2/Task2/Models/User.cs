using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string PasswordSalt { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
}