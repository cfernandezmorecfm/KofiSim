using UnityEngine;

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
        ShopPanelUI.Instance.Show(manager.CurrentDay); // Mostramos el panel de la tienda
    }

    public void Execute() {}
    public void Exit()
    {
        ShopPanelUI.Instance.Hide(); // Ocultamos el panel de la tienda al salir del estado
        Debug.Log("Saliendo de la tienda.");
    }
}

