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
        customer.UI.InitializePatience(consumeTime); // Reutilizamos la barra de paciencia para mostrar el tiempo de consumo
        Debug.Log("Cliente: estoy bebiendo mi consumición");
    }

    public void Execute()
    {
        consumeTimer += Time.deltaTime;
        float remaining= consumeTime - consumeTimer; // Calculamos el tiempo restante para mostrar en la barra de consumo
        customer.UI.UpdatePatience(remaining); // Actualizamos la barra de consumo con el tiempo restante

        if (consumeTimer >= consumeTime)
        {
            customer.UI.HideAll(); // Ocultamos la barra de consumo al terminar
            customer.ChangeState(new LeavingState(customer));
        }
    }

    public void Exit()
    {
        Debug.Log("Cliente: he terminado de consumir");
    }
}