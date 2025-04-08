using VetClinic.Api.Models;

namespace VetClinic.Api.Data;

public static class AnimalsRepository
{
    public static readonly List<Animal> Animals = 
    [
        new() { Id = 1, Name = "Fluffy", Category = "Cat", Weight = 4.5, FurColor = "White" },
        new() { Id = 2, Name = "Bruno", Category = "Dog", Weight = 20.3, FurColor = "Brown" },
        new() { Id = 3, Name = "Whiskers", Category = "Cat", Weight = 3.9, FurColor = "Gray" },
        new() { Id = 4, Name = "Rex", Category = "Dog", Weight = 25.7, FurColor = "Black" },
        new() { Id = 5, Name = "Chirpy", Category = "Bird", Weight = 0.2, FurColor = "Yellow" },
        new() { Id = 6, Name = "Nibbles", Category = "Rabbit", Weight = 2.1, FurColor = "White" },
        new() { Id = 7, Name = "Spike", Category = "Hedgehog", Weight = 0.9, FurColor = "Brown" },
        new() { Id = 8, Name = "Snowball", Category = "Rabbit", Weight = 1.8, FurColor = "White" },
        new() { Id = 9, Name = "Midnight", Category = "Cat", Weight = 4.2, FurColor = "Black" },
        new() { Id = 10, Name = "Sunny", Category = "Bird", Weight = 0.3, FurColor = "Green" }
    ];
}