using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure;

public class PrescriptionContext : DbContext
{
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected PrescriptionContext()
    {
    }

    public PrescriptionContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Smith", Email = "anna.smith@example.com" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Alice", LastName = "Brown", BirthDate = new DateOnly(1990, 5, 12) },
            new Patient { IdPatient = 2, FirstName = "Bob", LastName = "Green", BirthDate = new DateOnly(1985, 8, 20) }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Ibuprofen", Description = "Pain reliever", Type = "Tablet" },
            new Medicament { IdMedicament = 2, Name = "Paracetamol", Description = "Fever reducer", Type = "Tablet" }
        );

        modelBuilder.Entity<Prescription>().HasData(
            new Prescription { IdPrescription = 1, Date = new DateOnly(2025, 6, 1), DueDate = new DateOnly(2025, 6, 15), IdPatient = 1, IdDoctor = 1 },
            new Prescription { IdPrescription = 2, Date = new DateOnly(2025, 6, 2), DueDate = new DateOnly(2025, 6, 20), IdPatient = 2, IdDoctor = 2 }
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 1, Dose = 200, Details = "Take 2 times a day" },
            new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 2, Dose = 500, Details = "Take after meals" },
            new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 2, Dose = 500, Details = "Take 3 times a day" }
        );
    }
}