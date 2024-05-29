using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class Station
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
    [Required]
    public int Capacity { get; set; }
}