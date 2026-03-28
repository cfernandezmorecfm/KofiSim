using System.Collections.Generic; // Para usar la clase Queue
using UnityEngine;

public class OrderQueue : MonoBehaviour
{
    private Queue<CustomerFSM> pendingOrders = new Queue<CustomerFSM>(); // Cola FIFO para almacenar los pedidos de los clientes

    public int OrderCount { get {  return pendingOrders.Count; } }

    public void AddOrder(CustomerFSM customer)
    {
        // Guardar el pedido del cliente en la cola, asegurándose de que se mantenga el orden de llegada
        pendingOrders.Enqueue(customer);
        Debug.Log("Pedido añadido a la cola. Total de pedidos: " + pendingOrders.Count);
    }

    public CustomerFSM GetNextOrder()
    {
        // Obtener el siguiente pedido de la cola, asegurándose de que se atiendan en el orden correcto
        if (pendingOrders.Count > 0)
        {
            return pendingOrders.Dequeue();
        }
        Debug.Log("No hay pedidos en la cola.");
        return null;
    }
}
