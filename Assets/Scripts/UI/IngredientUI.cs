using System.Collections.Generic;
using UnityEngine;

public class IngredientUI : MonoBehaviour
{
    [Header("Desplegable")]
    [SerializeField] private GameObject contentPanel;    // el panel que se muestra/oculta
    [SerializeField] private Transform rowContainer;     // dónde se instancian las filas
    [SerializeField] private IngredientRowUI rowPrefab;  // prefab de una fila

    private readonly Dictionary<IngredientSO, IngredientRowUI> rows = new();
    private bool isOpen = false;

    private System.Action<IngredientStockChangedEvent> stockChangedHandler;

    private void OnEnable()
    {
        stockChangedHandler = OnStockChanged;
        EventBus.Subscribe(stockChangedHandler);

        BuildRows();      // una fila por ingrediente del stock actual
        SetOpen(false);   // empieza colapsado
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(stockChangedHandler);
    }

    // Crea (o recrea) las filas a partir del stock actual.
    private void BuildRows()
    {
        foreach (IngredientRowUI row in rows.Values)
        {
            if (row != null) Destroy(row.gameObject);
        }
        rows.Clear();

        foreach (KeyValuePair<IngredientSO, float> entry in IngredientManager.Instance.Stock)
        {
            IngredientRowUI row = Instantiate(rowPrefab, rowContainer);
            row.Set(entry.Key, entry.Value);
            rows[entry.Key] = row;
        }
    }

    private void OnStockChanged(IngredientStockChangedEvent evt)
    {
        // Si ya hay fila para ese ingrediente, la actualizamos; si no, reconstruimos.
        if (rows.TryGetValue(evt.Ingredient, out IngredientRowUI row) && row != null)
        {
            row.Set(evt.Ingredient, evt.NewAmount);
        }
        else
        {
            BuildRows();
        }
    }

    // Lo llama el botón "Ingredientes" en su OnClick.
    public void Toggle()
    {
        SetOpen(!isOpen);
    }

    private void SetOpen(bool open)
    {
        isOpen = open;
        if (contentPanel != null) contentPanel.SetActive(open);
    }
}
