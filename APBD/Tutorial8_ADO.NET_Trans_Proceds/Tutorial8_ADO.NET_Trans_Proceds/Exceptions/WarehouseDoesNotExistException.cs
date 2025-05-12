namespace Tutorial8_ADO.NET_Trans_Proceds.Exceptions;

public class WarehouseDoesNotExistException(int warehouseId) : Exception($"Warehouse {warehouseId} does not exist.");