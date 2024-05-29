using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class DroneToStation
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int DroneId { get; set; }
    [Required]
    public int StationId { get; set; }
}