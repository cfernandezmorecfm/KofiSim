using UnityEngine;

public class WaitingForOrderState : ICustomerState
{
    private CustomerFSM customer;
    private BaristaWorker barista; // Referencia al barista

    public WaitingForOrderState(CustomerFSM customer)
    {
        this.customer = customer;
    }

    public void Enter()
    {
        barista = Object.FindAnyObjectByType<BaristaWorker>(); // Encontramos al barista en la escena para poder registrar cafés sobrantes si el cliente se va sin pagar
        customer.UI.ShowWaitingIcon(true); // Mostramos el icono de espera (burbuja de pensamiento)
        Debug.Log("Cliente: esperando mi café");
    }

    public void FixedExecute()
    {
        // El cliente se queda quieto esperando su pedido, no hay movimiento en esta fase
    }
    public void Execute()
    {
        customer.ReducePatience(Time.deltaTime);
        float remaining = customer.CurrentPatience / customer.PatienceForReceiveOrder; // Calculamos el tiempo restante como un valor entre 0 y 1
        customer.UI.UpdateWaitingForOrderIcon(remaining); // Actualizamos el icono de espera con el tiempo restante

        if (customer.CurrentPatience <= 0f)
        {
            Debug.Log("Cliente: No me traen el pedido, me voy sin pagar");
            barista.RegisterSurplusCoffee(); // Registramos un café sobrante para el barista, ya que el cliente se va sin pagar
            customer.ChangeState(new LeavingState(customer, false)); // Cambiamos al estado de salida, pasando false para indicar que el cliente no estaba satisfecho
        }
    }
    
    public void Exit()
    {
        customer.UI.ShowWaitingIcon(false);
        Debug.Log("Cliente: me traen mi café");
    }
}