namespace Tutorial8_ADO.NET_Trans_Proceds.Exceptions;

public class ProductDoesNotExistException(int productId) : Exception($"Product {productId} does not exist.");