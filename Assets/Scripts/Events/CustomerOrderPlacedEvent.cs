public readonly struct CustomerOrderPlacedEvent
{
    public CustomerFSM Customer { get; }

    public CustomerOrderPlacedEvent(CustomerFSM customer)
    {
        // Pasamos el cliente que ha realizado el pedido para que los sistemas que escuchen este evento puedan acceder a la información del cliente y su pedido.
        Customer = customer;
    }
}