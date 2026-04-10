using UnityEngine;
using System.Collections;

public class ShoppingState : IDayCycleState
{     
    private DayCycleManager manager;
    public ShoppingState(DayCycleManager manager)
    {
        this.manager = manager;
    }
    public void Enter()
    {
        Debug.Log("Entrando a la tienda.");
        
        manager.StartCoroutine(AutoAdvance());
    }

    private IEnumerator AutoAdvance()
    {
        // Esperamos 5 segundos antes de avanzar automáticamente
        yield return new WaitForSeconds(5f);
        manager.AdvanceToNextDay();
        manager.ChangeState(new ServiceState(manager));
    }
    public void Execute() {}
    public void Exit()
    {
        Debug.Log("Saliendo de la tienda.");
    }
}

