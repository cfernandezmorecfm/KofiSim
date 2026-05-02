using UnityEngine;
using System;
using UnityEditor;
public class ClosingState : IDayCycleState
{
    private DayCycleManager manager;
    private int activeCustomers;

    private Action<CustomerSpawnedEvent> customerSpawnedHandler;
    private Action<CustomerLeftEvent> customerLeftHandler;
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
        manager.CustomerSpawner.SetSpawningEnabled(false);
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

    private void OnCustomerSpawned(CustomerSpawnedEvent evt)
    {
        activeCustomers++;
    }

    private void OnCustomerLeft(CustomerLeftEvent evt)
    {
        activeCustomers--;
    }
    
}