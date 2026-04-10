using UnityEngine;
using System.Collections; // Para usar IEnumerator y WaitForSeconds
public class SummaryState : IDayCycleState
{
    private DayCycleManager manager;
    public SummaryState(DayCycleManager manager)
    {
        this.manager = manager;
    }
    public void Enter()
    {
        Debug.Log($"Día {manager.CurrentDay}: Resumen del día");

        // Pagamos salarios, actualizamos estadísticas, etc.
        float salary = manager.Barista.Salary;
        MoneyManager.Instance.SpendMoney(salary);

        float income = manager.DayIncome;
        float balance = income - salary;

        Debug.Log($"Ingresos: {income}, Salarios: {salary}, Resultado neto: {balance}");

        manager.StartCoroutine(AutoAdvance());

    }

    private IEnumerator AutoAdvance()
    {
        // Esperamos 5 segundos antes de avanzar automáticamente
        yield return new WaitForSeconds(5f);
        manager.ChangeState(new ShoppingState(manager));
    }
    public void Execute() { }
    public void Exit()
    {
        Debug.Log($"Día {manager.CurrentDay} - Fin del balance");
    }

}
