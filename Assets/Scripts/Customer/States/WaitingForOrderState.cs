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
        Debug.Log("Cliente: esperando mi café");
    }

    public void Execute()
    {
        // Futura implementación, el jugador traerá el café.
    }

    public void Exit()
    {
        Debug.Log("Cliente: me traen mi café");
    }
}