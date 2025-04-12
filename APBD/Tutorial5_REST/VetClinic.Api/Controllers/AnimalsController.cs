using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Contracts.Requests;
using VetClinic.Api.Data;
using VetClinic.Api.Models;

namespace VetClinic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly List<Animal> _animals = AnimalsRepository.Animals;

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_animals);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult Create(CreateAnimalRequest request)
    {
        var newAnimal = new Animal
        {
            Name = request.Name, 
            Category = request.Category, 
            Weight = request.Weight,
            FurColor = request.FurColor
        };
        
        _animals.Add(newAnimal);
        return CreatedAtAction(nameof(GetById), new { id = newAnimal.Id }, newAnimal);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateAnimalRequest request)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        
        animal.Name = request.Name;
        animal.Category = request.Category;
        animal.Weight = request.Weight;
        animal.FurColor = request.FurColor;
        
        return Ok(animal);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        
        _animals.Remove(animal);
        return NoContent();
    }
}