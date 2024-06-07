using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class Acceleration
{
    [Key]
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string SerialNumber { get; set; }
}