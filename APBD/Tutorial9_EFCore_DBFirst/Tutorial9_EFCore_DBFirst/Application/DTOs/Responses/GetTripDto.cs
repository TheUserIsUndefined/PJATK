namespace Tutorial9_EFCore_DBFirst.DTOs.Responses;

public class GetTripDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public virtual ICollection<CountryDto> Countries { get; set; } = [];
    public virtual ICollection<ClientDto> Clients { get; set; } = [];
}