using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class DroneModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public float Price { get; set; }
}