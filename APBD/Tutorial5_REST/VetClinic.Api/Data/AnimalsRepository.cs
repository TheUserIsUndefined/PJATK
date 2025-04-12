using VetClinic.Api.Models;

namespace VetClinic.Api.Data;

public static class AnimalsRepository
{
    public static readonly List<Animal> Animals = 
    [
        new() { Name = "Fluffy", Category = "Cat", Weight = 4.5, FurColor = "White" },
        new() { Name = "Bruno", Category = "Dog", Weight = 20.3, FurColor = "Brown" },
        new() { Name = "Whiskers", Category = "Cat", Weight = 3.9, FurColor = "Gray" },
        new() { Name = "Rex", Category = "Dog", Weight = 25.7, FurColor = "Black" },
        new() { Name = "Chirpy", Category = "Bird", Weight = 0.2, FurColor = "Yellow" },
        new() { Name = "Nibbles", Category = "Rabbit", Weight = 2.1, FurColor = "White" },
        new() { Name = "Spike", Category = "Hedgehog", Weight = 0.9, FurColor = "Brown" },
        new() { Name = "Snowball", Category = "Rabbit", Weight = 1.8, FurColor = "White" },
        new() { Name = "Midnight", Category = "Cat", Weight = 4.2, FurColor = "Black" },
        new() { Name = "Sunny", Category = "Bird", Weight = 0.3, FurColor = "Green" }
    ];
}