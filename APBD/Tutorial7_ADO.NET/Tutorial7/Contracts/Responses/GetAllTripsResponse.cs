namespace Tutorial7.Contracts.Responses;

public class GetAllTripsResponse
{
    public int IdTrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public HashSet<string> Countries { get; set; }
}