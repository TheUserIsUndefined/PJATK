namespace Tutorial8_ADO.NET_Trans_Proceds.Exceptions;

public class OrderHasBeenCompletedException(int orderId) : Exception($"Order {orderId} has been already completed.");