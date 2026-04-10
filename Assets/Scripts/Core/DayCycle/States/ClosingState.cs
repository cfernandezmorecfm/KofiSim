using UnityEngine;

public class ClosingState : IDayCycleState
{
    private DayCycleManager manager;

    public ClosingState(DayCycleManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log($"Día {manager.CurrentDay}: El turno ha terminado");
        manager.CustomerSpawner.SetSpawningEnabled(false);
    }

    public void Execute()
    {
        // Esperamos a que todos los clientes hayan sido atendidos o se hayan ido
        if (CustomerFSM.ActiveCount == 0)
        {
            manager.ChangeState(new SummaryState(manager));
        }

    }

    public void Exit()
    {
        Debug.Log($"Día {manager.CurrentDay}: Todos los clientes se han ido");
    }

}