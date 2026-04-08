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
        customer.UI.ShowConsumingIcon(true); // Mostramos el icono de consumo
        Debug.Log("Cliente: estoy bebiendo mi consumición");
    }

    public void Execute()
    {
        consumeTimer += Time.deltaTime;
        float remaining = 1f - (consumeTimer / consumeTime); // Calculamos el tiempo restante como un valor entre 0 y 1
        customer.UI.UpdateConsumingIcon(remaining); // Actualizamos la barra de consumo con el tiempo restante

        if (consumeTimer >= consumeTime)
        {
            customer.UI.HideAll(); // Ocultamos la barra de consumo al terminar
            customer.ChangeState(new LeavingState(customer, true)); // Cambiamos al estado de salida, pasando true para indicar que el cliente estaba satisfecho
        }
    }

    public void Exit()
    {
        Debug.Log("Cliente: he terminado de consumir");
    }
}