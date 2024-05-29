using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class DroneStatus
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int NumericName { get; set; }
    [Required]
    public string Description { get; set; }
    
    public enum Status
    {
        Idle = 1,
        Rented = 2,
        Rest = 3
    }
}