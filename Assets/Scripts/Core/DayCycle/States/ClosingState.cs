using UnityEngine;
using System;
using UnityEditor;
public class ClosingState : IDayCycleState
{
    private DayCycleManager manager;
 

    public DayPhase Phase => DayPhase.Closing; // Implementamos la propiedad Phase para identificar este estado como el de cierre
    public ClosingState(DayCycleManager manager)
    {
        this.manager = manager;
    }
    public void Enter()
    {

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
    
}