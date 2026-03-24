using UnityEngine;

public class LeavingState : ICustomerState
{
    private CustomerFSM customer;
    private Vector2 exitPosition;

    public LeavingState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        customer.FreeSeat();
        // Salir por la izquierda, misma posición que el SpawnPoint
        exitPosition = new Vector2(-10f, customer.Rb.position.y);
        Debug.Log("Cliente: pagando y yéndome");
    }

    public void Execute()
    {
        float distance = Vector2.Distance(customer.Rb.position, exitPosition);

        if (distance <= 0.05f)
        {
            customer.DestroySelf();
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(customer.Rb.position, exitPosition, customer.MoveSpeed * Time.deltaTime);
            customer.Rb.MovePosition(newPos);
        }
    }

    public void Exit()
    {
        // No se ejecutará porque el cliente se destruye
    }
}