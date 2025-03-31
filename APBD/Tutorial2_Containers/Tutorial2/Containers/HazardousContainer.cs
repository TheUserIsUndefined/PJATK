using Tutorial2.Interfaces;
using Tutorial2.Exceptions;

namespace Tutorial2.Containers;


public abstract class HazardousContainer(char typeCode, double height, double tareWeight, double depth, double maxPayload) :
    Container(typeCode, height, tareWeight, depth, maxPayload), IHazardNotifier
{
    
    public override void LoadCargo(double cargoWeight)
    {
        if (cargoWeight < 0)
            throw new ArgumentOutOfRangeException(nameof(cargoWeight), "CargoWeight cannot be negative.");
        
        if (!ValidateWeight(cargoWeight))
        {
            NotifyHazard("Hazardous cargo limit exceeded");
            throw new OverfillException();
        }
        
        CargoWeight += cargoWeight;
    }

    public void NotifyHazard(string message)
    {
        Console.WriteLine($"Hazard situation occured on {SerialNumber} container. Message:\n{message}");
    }
}