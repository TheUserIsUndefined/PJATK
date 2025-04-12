namespace VetClinic.Api.Models;

public class Animal
{
    private static int _counter;
    public int Id { get; } = ++_counter;
    public string Name { get; set; }
    public string Category { get; set; }
    public double Weight { get; set; }
    public string FurColor { get; set; }
}