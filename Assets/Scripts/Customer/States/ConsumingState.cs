using UnityEngine;

public class ConsumingState : ICustomerState
{
    private CustomerFSM customer;
    private float consumeTimer = 0f;
    private float consumeTime = 5f;

    public ConsumingState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        consumeTimer = 0f;
        Debug.Log("Cliente: estoy bebiendo mi consumición");
    }

    public void Execute()
    {
        consumeTimer += Time.deltaTime;

        if (consumeTimer >= consumeTime)
        {
            customer.ChangeState(new LeavingState(customer));
        }
    }

    public void Exit()
    {
        Debug.Log("Cliente: he terminado de consumir");
    }
}