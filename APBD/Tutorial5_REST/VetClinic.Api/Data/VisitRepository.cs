using VetClinic.Api.Models;

namespace VetClinic.Api.Data;

public static class VisitRepository
{
    public static readonly List<Visit> Visits =
    [
        new()
        {
            VisitDate = "2024-09-01",
            Animal = AnimalsRepository.Animals[0],
            Description = "Annual check-up and vaccinations",
            Price = 75.0
        },
        new()
        {
            VisitDate = "2024-09-05",
            Animal = AnimalsRepository.Animals[1],
            Description = "Limping on front leg, X-ray performed",
            Price = 120.0
        },
        new()
        {
            VisitDate = "2024-09-10",
            Animal = AnimalsRepository.Animals[2],
            Description = "Dental cleaning",
            Price = 60.0
        },
        new()
        {
            VisitDate = "2024-09-12",
            Animal = AnimalsRepository.Animals[3],
            Description = "Ear infection treatment",
            Price = 90.0
        },
        new()
        {
            VisitDate = "2024-09-15",
            Animal = AnimalsRepository.Animals[4],
            Description = "Broken wing evaluation",
            Price = 45.0
        },
        new()
        {
            VisitDate = "2024-09-20",
            Animal = AnimalsRepository.Animals[5],
            Description = "Nail trimming and grooming",
            Price = 35.0
        },
        new()
        {
            VisitDate = "2024-09-22",
            Animal = AnimalsRepository.Animals[6],
            Description = "Skin rash consultation",
            Price = 50.0
        },
        new()
        {
            VisitDate = "2024-09-25",
            Animal = AnimalsRepository.Animals[7],
            Description = "Vaccination booster",
            Price = 40.0
        },
        new()
        {
            VisitDate = "2024-09-28",
            Animal = AnimalsRepository.Animals[8],
            Description = "Check-up after surgery",
            Price = 110.0
        },
        new()
        {
            VisitDate = "2024-09-30",
            Animal = AnimalsRepository.Animals[9],
            Description = "Beak trimming and feather check",
            Price = 30.0
        }
    ];
}
