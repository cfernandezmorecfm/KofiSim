using UnityEngine;

public class ServiceState : IDayCycleState
{
    private DayCycleManager manager;
    private float timer;

    public ServiceState(DayCycleManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log($"Día {manager.CurrentDay} — SERVICIO: empieza el turno");
        manager.ResetDayIncome();
        manager.CustomerSpawner.SetSpawningEnabled(true);
        timer = manager.DayDurationInSeconds;
        manager.NotifyTimerChanged(timer);
    }

    public void Execute()
    {
        timer -= Time.deltaTime;
        manager.NotifyTimerChanged(timer);

        if (timer <= 0f)
        {
            manager.ChangeState(new ClosingState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log($"Día {manager.CurrentDay} — SERVICIO terminado");
    }

}
