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
        customer.UI.ShowWaitingIcon(true); // Reutilizamos el icono de espera para indicar que el cliente está esperando su pedido
        Debug.Log("Cliente: esperando mi café");
    }

    public void Execute()
    {
        // Futura implementación, el jugador traerá el café.
    }

    public void Exit()
    {
        customer.UI.ShowWaitingIcon(false);
        Debug.Log("Cliente: me traen mi café");
    }
}