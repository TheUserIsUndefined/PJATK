using Microsoft.AspNetCore.Mvc;
using VetClinic.Api.Contracts.Requests;
using VetClinic.Api.Data;
using VetClinic.Api.Models;

namespace VetClinic.Api.Controllers;

[ApiController]
[Route("api/animals/{animalId:int}/[controller]")]

public class VisitsController : ControllerBase
{
    private readonly List<Visit> _visits = VisitRepository.Visits;
    private readonly List<Animal> _animals = AnimalsRepository.Animals;

    [HttpGet]
    public IActionResult Get(int animalId)
    {
        var visits = _visits
            .Where(visit => visit.Animal.Id == animalId)
            .ToList();
        
        if (visits.Count == 0) return NotFound();
        
        return Ok(visits);
    }

    [HttpPost]
    public IActionResult Create(int animalId, CreateVisitRequest request)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == animalId);
        if (animal is null) return NotFound();

        var newVisit = new Visit
        {
            Animal = animal,
            VisitDate = request.VisitDate,
            Description = request.Description,
            Price = request.Price
        };
        
        _visits.Add(newVisit);
        return Ok(newVisit);
    }
}