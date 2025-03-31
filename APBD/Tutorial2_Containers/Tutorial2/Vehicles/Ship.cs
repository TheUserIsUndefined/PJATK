using Tutorial2.Containers;

namespace Tutorial2.Vehicles;

public class Ship(double maxSpeed, int maxNumberOfContainers, double maxWeight)
{
    public double MaxSpeed { get; set; } = maxSpeed;
    public int MaxNumberOfContainers { get; set; } = maxNumberOfContainers;
    public double MaxWeight { get; set; } = maxWeight;
    public List<Container> LoadedContainers { get; set; } = new List<Container>();
    
    public void ReplaceContainer(string oldContainerNumber, Container newContainer)
    {
        if (oldContainerNumber == newContainer.SerialNumber)
        {
            Console.WriteLine($"Cannot replace container {oldContainerNumber} with itself.");
            return;
        }
        
        var oldContainer = LoadedContainers.FirstOrDefault(c => c.SerialNumber == oldContainerNumber);
        if (oldContainer == null)
        {
            Console.WriteLine($"Container {oldContainerNumber} not found");
            return;
        }
        
        LoadedContainers.Remove(oldContainer);
        if (!LoadContainer(newContainer, false))
        {
            LoadedContainers.Add(oldContainer);
            return;
        }
        
        Console.WriteLine($"Container {oldContainerNumber} replaced with {newContainer.SerialNumber}.");
    }

    public void LoadContainers(List<Container> containers)
    {
        foreach (var container in containers)
        {
            LoadContainer(container);
            
            if (LoadedContainers.Count == MaxNumberOfContainers)
            {
                Console.WriteLine($"Maximum number of containers reached, containers after {container.SerialNumber} won't be loaded.");
                return;
            }
        }
    }

    public bool LoadContainer(Container container, bool printSuccessMsg = true)
    {
        string contNumber = container.SerialNumber;
        if (LoadedContainers.Contains(container))
            Console.WriteLine($"Container {contNumber} already loaded");
        else if (LoadedContainers.Count == MaxNumberOfContainers)
            Console.WriteLine($"Maximum number of containers reached, container {contNumber} won't be loaded.");
        else if (LoadedContainers.Sum(c => c.TotalMass()) + container.TotalMass() > MaxWeight*1000)
            Console.WriteLine($"Maximum weight will be exceeded. Container {contNumber} won't be loaded.");
        else
        {
            LoadedContainers.Add(container);
            if (printSuccessMsg)
                Console.WriteLine($"Container {contNumber} loaded.");
            return true;
        }

        return false;
    }

    public void UnloadContainer(Container container, bool printSuccessMsg = true)
    {
        string contNumber = container.SerialNumber;
        if (!LoadedContainers.Contains(container))
        {
            Console.WriteLine($"No container {contNumber} on the {GetType().Name} found.");
            return;
        }
        
        LoadedContainers.Remove(container);
        
        if (printSuccessMsg)
            Console.WriteLine($"Container {contNumber} unloaded.");
    }

    public void PrintInformation()
    {
        Console.WriteLine(this);
        Console.WriteLine("Containers:");
        foreach(var container in LoadedContainers)
            Console.WriteLine(container);
    }
    
    
    public static void TransferContainerBetweenShips(Ship shipFrom, Ship shipTo, Container container)
    {
        string contNumber = container.SerialNumber;
        shipFrom.UnloadContainer(container, false);
        var isLoaded = shipTo.LoadContainer(container, false);
        if (!isLoaded)
        {
            Console.WriteLine("The transfer has been interrupted.");
            shipFrom.LoadContainer(container, false);
        }
        else 
            Console.WriteLine($"Container {contNumber} successfully transferred.");

    }
    
    public override string ToString()
    {
        return $"{GetType().Name}:\nMax Speed: {MaxSpeed},\nMax Number of Containers: {MaxNumberOfContainers},\nMaximum weight of all containers: {MaxWeight}";
    }
}