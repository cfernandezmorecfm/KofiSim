using UnityEngine;

public class WaitingForOrderState : ICustomerState
{
    private CustomerFSM customer;

    public WaitingForOrderState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        customer.UI.ShowWaitingIcon(true); // Mostramos el icono de espera (burbuja de pensamiento)
        Debug.Log("Cliente: esperando mi café");
    }

    public void FixedExecute()
    {
        // El cliente se queda quieto esperando su pedido, no hay movimiento en esta fase
    }
    public void Execute()
    {
        // Posible futura implementación
    }
    
    public void Exit()
    {
        customer.UI.ShowWaitingIcon(false);
        Debug.Log("Cliente: me traen mi café");
    }
}