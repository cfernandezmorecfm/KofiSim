using UnityEngine;

public class ArrivingState : ICustomerState
{
    private CustomerFSM customer;

    public ArrivingState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        Debug.Log("Cliente: caminando hacia mi asiento");
    }

    public void Execute() { }
    public void FixedExecute()
    {
        Vector2 seatPos = customer.TargetSeat.Position;
        float distance = Vector2.Distance(customer.Rb.position, seatPos);

        if (distance <= 0.05f)
        {
            customer.Rb.position = seatPos;
            customer.ChangeState(new WaitingForServiceState(customer));
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(customer.Rb.position, seatPos, customer.MoveSpeed * Time.fixedDeltaTime);
            customer.Rb.MovePosition(newPos);
        }
    }



    public void Exit()
    {
        Debug.Log("Cliente: he llegado a mi asiento");
    }
}