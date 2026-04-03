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
        customer.UI.InitializePatience(customer.Patience); // Inicializamos la barra de paciencia con el valor máximo
        Debug.Log("Cliente: sentado, esperando servicio");
    }

    public void Execute()
    {
        customer.ReducePatience(Time.deltaTime);
        customer.UI.UpdatePatience(customer.CurrentPatience); // Actualizamos la barra de paciencia con el valor actual

        if (customer.CurrentPatience <= 0f)
        {
            Debug.Log("Cliente: me he cansado de esperar, me voy");
            customer.UI.HideAll(); // Ocultamos la barra de paciencia y cualquier otro elemento relacionado
            customer.ChangeState(new LeavingState(customer));
        }
    }

    public void Exit()
    {
        customer.UI.ShowPatienceBar(false); // Ocultamos la barra de paciencia al salir del estado
        Debug.Log("Cliente: me están atendiendo");
    }
}