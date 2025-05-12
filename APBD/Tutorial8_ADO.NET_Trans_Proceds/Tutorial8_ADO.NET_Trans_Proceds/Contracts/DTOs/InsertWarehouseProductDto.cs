namespace Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;

public class InsertWarehouseProductDto
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public double Price { get; set; }
}