using UnityEngine;

public class LeavingState : ICustomerState
{
    private CustomerFSM customer;
    private Vector2 exitPosition;
    private bool wasSatisfied; // Variable para almacenar si el cliente estaba satisfecho o no y controlar si nos debe pagar o no

    public LeavingState(CustomerFSM customer, bool wasSatisfied) // Pasamos el estado de satisfacción del cliente al constructor para decidir si paga o no
    {
        this.customer = customer;
        this.wasSatisfied = wasSatisfied;
    }

    public void Enter()
    {
        customer.FreeSeat();
        
        if(wasSatisfied)
        {
            float price = MoneyManager.Instance.CoffeePrice;
            MoneyManager.Instance.AddMoney(price); // El cliente paga solo si estaba satisfecho
            Debug.Log($"Cliente: Estoy satisfecho, voy a pagar ${price:F2}");
            DayCycleManager.Instance.AddIncome(price); // Registramos el pago en el DayCycleManager para llevar un conteo de las ganancias del día
            DayCycleManager.Instance.IncrementCoffeeSold(); // Incrementamos la cantidad de cafés vendidos en el día actual
        }
        else
        {
            Debug.Log("Cliente: No estoy satisfecho, no voy a pagar");
        }

        // Salir por la izquierda, misma posición que el SpawnPoint
        exitPosition = new Vector2(-10f, customer.Rb.position.y);
    }

    public void Execute()
    {
        // No se ejecutará porque el cliente se mueve en FixedUpdate
    }
    public void FixedExecute()
    {
        float distance = Vector2.Distance(customer.Rb.position, exitPosition);

        if (distance <= 0.05f)
        {
            customer.DestroySelf();
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(customer.Rb.position, exitPosition, customer.MoveSpeed * Time.fixedDeltaTime);
            customer.Rb.MovePosition(newPos);
        }
    }

    public void Exit()
    {
        // No se ejecutará porque el cliente se destruye
    }
}