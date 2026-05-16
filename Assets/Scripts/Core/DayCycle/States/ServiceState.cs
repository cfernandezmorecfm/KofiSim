using UnityEngine;

public class ServiceState : IDayCycleState
{
    private DayCycleManager manager;
    private float timer;

    public DayPhase Phase => DayPhase.Service; // Implementamos la propiedad Phase para identificar este estado como el de servicio
    public ServiceState(DayCycleManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Time.timeScale = 1f; // Quitamos la pausa del juego para empezar el servicio
        Debug.Log($"Día {manager.CurrentDay} — SERVICIO: empieza el turno");
        manager.ResetDayIncome();
        manager.CustomerSpawner.SetSpawningEnabled(true);
        timer = manager.DayDurationInSeconds;
    }

    public void Execute()
    {
        timer -= Time.deltaTime;

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
