using Tutorial2.Exceptions;
using Tutorial2.Interfaces;

namespace Tutorial2.Containers;

public class LiquidContainer : HazardousContainer
{
    private readonly double _allowedMaxPayload;
    
    public LiquidContainer(double height, double tareWeight, double depth, double maxPayload, bool isHazardous)
        : base('L', height, tareWeight, depth, maxPayload)
    {
        IsHazardous = isHazardous;
        _allowedMaxPayload = MaxPayload * (isHazardous ? 0.5 : 0.9);
    }
    
    public override bool ValidateWeight(double weightToAdd) => CargoWeight + weightToAdd <= _allowedMaxPayload;
    
    public bool IsHazardous { get; }
}