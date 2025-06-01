using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial10_EFCore_CodeFirst.Core.Models;

public class PrescriptionMedicament
{
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    
    public int? Dose { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Details { get; set; }
    
    public Prescription Prescription { get; set; }
    public Medicament Medicament { get; set; }
}