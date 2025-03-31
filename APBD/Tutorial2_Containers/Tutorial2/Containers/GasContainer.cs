using Tutorial2.Interfaces;

namespace Tutorial2.Containers;

public class GasContainer(double height, double tareWeight, double depth, double maxPayload, double pressure)
    : HazardousContainer('G', height, tareWeight, depth, maxPayload)
{
    public double Pressure { get; } = pressure;
    
    public override void EmptyTheCargo()
    {
        CargoWeight *= 0.05;
    }
}