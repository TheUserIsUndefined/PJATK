using System.ComponentModel.DataAnnotations;

namespace Test.Contracts.Requests;

public class InsertTaskRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    
    [Required]
    public DateTime Deadline { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProject { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTaskType { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdAssignedTo { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdCreator { get; set; }
}