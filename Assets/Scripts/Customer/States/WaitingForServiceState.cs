using UnityEngine;

public class WaitingForServiceState : ICustomerState
{
    private CustomerFSM customer;

    public WaitingForServiceState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        Debug.Log("Cliente: sentado, esperando servicio");
    }

    public void Execute()
    {
        customer.ReducePatience(Time.deltaTime);

        if (customer.CurrentPatience <= 0f)
        {
            Debug.Log("Cliente: me he cansado de esperar, me voy");
            customer.ChangeState(new LeavingState(customer));
        }
    }

    public void Exit()
    {
        Debug.Log("Cliente: me están atendiendo");
    }
}