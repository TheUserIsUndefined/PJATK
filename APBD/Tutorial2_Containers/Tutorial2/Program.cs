using Tutorial2.Containers;
using Tutorial2.Vehicles;

public class Program
{
    public static void Main(string[] args)
    {
        Container conL1 = new LiquidContainer(1, 100, 1, 1000, true);
        Container conG = new GasContainer(1, 100, 1, 1000, 5);
        Container conR = new RefrigeratedContainer(1, 100, 1, 1000, "Fish", 5);
        Container conL2 = new LiquidContainer(1, 1, 1, 20000, true);
        Ship ship1 = new Ship(10, 5, 10);
        Ship ship2 = new Ship(10, 5, 10);
        Ship ship3 = new Ship(10, 5, 1);
        
        conL1.LoadCargo(500);
        conG.LoadCargo(1000);
        conR.LoadCargo(1000);
        conL2.LoadCargo(10000);
        
        ship1.LoadContainer(conL1);
        ship2.LoadContainers([conG, conR]);
        
        Ship.TransferContainerBetweenShips(ship2, ship1, conG);
        Ship.TransferContainerBetweenShips(ship1, ship3, conG);
        
        ship3.LoadContainer(conL2);
        
        ship1.ReplaceContainer("KON-L-1", conL2);
        
        conG.EmptyTheCargo();
        
        Console.WriteLine(conG);
        ship1.PrintInformation();
    }
}