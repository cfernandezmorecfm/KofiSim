using UnityEngine;
using System;
using UnityEditor;
public class ClosingState : IDayCycleState
{
    private DayCycleManager manager;
    private int activeCustomers; //* vestigio de la implementación anterior, ahora se usa manager.CustomerSpawner.ActiveCustomers directamente

    private Action<CustomerSpawnedEvent> customerSpawnedHandler;
    private Action<CustomerLeftEvent> customerLeftHandler;

    public DayPhase Phase => DayPhase.Closing; // Implementamos la propiedad Phase para identificar este estado como el de cierre
    public ClosingState(DayCycleManager manager)
    {
        this.manager = manager;
    }
    public void Enter()
    {
        // Pull inicial para sincronizar el contador con los clientes en escena
        activeCustomers = manager.CustomerSpawner.ActiveCustomers;

        customerSpawnedHandler = OnCustomerSpawned;
        customerLeftHandler = OnCustomerLeft;
        EventBus.Subscribe(customerSpawnedHandler);
        EventBus.Subscribe(customerLeftHandler);

        Debug.Log($"Día {manager.CurrentDay}: El turno ha terminado");
    }
    public void Execute()
    {
        // Esperamos a que todos los clientes hayan sido atendidos o se hayan ido
        if (manager.CustomerSpawner.ActiveCustomers == 0)
        {
            manager.ChangeState(new SummaryState(manager));
        }
    }
    public void Exit()
    {
        Debug.Log($"Día {manager.CurrentDay}: Todos los clientes se han ido");
    }

    //* Para corregir, trabajo duplicado, se puede leer directmente de manager.CustomerSpawner.ActiveCustomers
    private void OnCustomerSpawned(CustomerSpawnedEvent evt)
    {
        activeCustomers++;
    }

    private void OnCustomerLeft(CustomerLeftEvent evt)
    {
        activeCustomers--;
    }
    
}