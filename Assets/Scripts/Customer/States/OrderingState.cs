using UnityEngine;

public class OrderingState : ICustomerState
{
    private CustomerFSM customer;

    public OrderingState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        Debug.Log("Cliente: me están tomando el pedido");
    }

    public void Execute()
    {
        // La lógica del timer la gestiona el PlayerController
        // Este estado solo espera a que el jugador complete o cancele
    }

    public void Exit()
    {
        Debug.Log("Cliente: pedido tomado");
    }
}
