using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class Drone
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string SerialNumber { get; set; }
    [Required]
    public int ModelId { get; set; }
    [Required]
    public int StatusId { get; set; }
    [Required]
    public int CurrentUserId { get; set; }
}