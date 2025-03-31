using Tutorial2.Exceptions;

namespace Tutorial2.Containers;

public abstract class Container
{
    private static int IDCounter;
    
    protected Container(char typeCode, double height, double tareWeight, double depth, double maxPayload)
    {
        Height = height < 0 ? 
            throw new ArgumentOutOfRangeException(nameof(height), "Height cannot be negative.") : height;
        TareWeight = tareWeight < 0 ? 
            throw new ArgumentOutOfRangeException(nameof(tareWeight), "TareWeight cannot be negative.") : tareWeight;
        Depth = depth < 0 ? 
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth cannot be negative.") : depth;
        MaxPayload = maxPayload < 0 ? 
            throw new ArgumentOutOfRangeException(nameof(maxPayload), "MaxPayload cannot be negative.") : maxPayload;

        SerialNumber = $"KON-{typeCode}-{++IDCounter}";
    }
    
    public double Height { get; }
    public double TareWeight { get; }
    public double Depth { get; }
    public string SerialNumber { get; }
    public double MaxPayload { get; }
    public double CargoWeight { get; protected set; }
    
    public virtual void LoadCargo(double cargoWeight)
    {
        if (cargoWeight < 0)
            throw new ArgumentOutOfRangeException(nameof(cargoWeight), "CargoWeight cannot be negative.");
        
        if (!ValidateWeight(cargoWeight))
            throw new OverfillException("The mass of cargo is too large.");
        
        CargoWeight += cargoWeight;
    }

    public virtual void EmptyTheCargo() => CargoWeight = 0;

    public double TotalMass() => CargoWeight + TareWeight;
    
    public virtual bool ValidateWeight(double weightToAdd) => CargoWeight + weightToAdd <= MaxPayload;

    public override string ToString()
    {
        return $"Container {SerialNumber}:" +
               $"\nHeight: {Height}," +
               $"\nDepth: {Depth}," +
               $"\nTare Weight: {TareWeight}," +
               $"\nLoaded Cargo Weight: {CargoWeight}," +
               $"\nMax Payload: {MaxPayload}";
    }
}