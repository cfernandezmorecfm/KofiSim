using UnityEngine;
public class SummaryState : IDayCycleState
{
    private DayCycleManager manager;

    public DayPhase Phase => DayPhase.Summary; // Implementamos la propiedad Phase para identificar este estado como el de resumen del dia
    public SummaryState(DayCycleManager manager)
    {
        this.manager = manager;
    }
    public void Enter()
    {
        Time.timeScale = 0f; // Pausamos el juego para mostrar el resumen

        // Pagamos salarios, actualizamos estadísticas, etc.
        float salary = manager.Barista.Salary;
        MoneyManager.Instance.SpendMoney(salary);

    }

    public void Execute() { }
    public void Exit()
    {
        Debug.Log($"Día {manager.CurrentDay} - Fin del balance");
    }

}
