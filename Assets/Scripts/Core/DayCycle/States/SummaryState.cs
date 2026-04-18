using UnityEngine;
public class SummaryState : IDayCycleState
{
    private DayCycleManager manager;
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

        float income = manager.DayIncome;

        // Mostramos el panel de resumen con la información del día
        SummaryPanelUI.InstanceID.show(manager.CurrentDay, income, salary);

    }

    public void Execute() { }
    public void Exit()
    {
        SummaryPanelUI.InstanceID.Hide();
        Debug.Log($"Día {manager.CurrentDay} - Fin del balance");
    }

}
