namespace Tutorial8_ADO.NET_Trans_Proceds.Exceptions;

public class OrderDoesNotExistException(int productId, int amount) : 
    Exception($"Order for product {productId} and amount {amount} does not exist.");