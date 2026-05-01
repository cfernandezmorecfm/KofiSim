using UnityEngine;
using TMPro;
public class IngredientUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentCoffeeText;

    private System.Action<IngredientStockChangedEvent> stockChangedHandler;
    private void OnEnable()
    {
        stockChangedHandler = OnStockChanged; // Guardamos la referencia al handler para garantizar que Subscribe y Unsubscribe utilizan la misma
        EventBus.Subscribe(stockChangedHandler); // Suscribirse al evento de cambio de stock de café

        // Actualizar la UI con el stock inicial de café
        UpdateCoffeeText(IngredientManager.Instance.CurrentCoffeGrams);
    }

    private void OnDisable()
    {
       EventBus.Unsubscribe(stockChangedHandler); // Desuscribirse del evento para evitar fugas de memoria
    }

    private void OnStockChanged(IngredientStockChangedEvent evt)
    {
        UpdateCoffeeText(evt.NewGrams); // Actualizar la UI con el nuevo stock de café cada vez que se publique el evento
    }
    private void UpdateCoffeeText(float amount)
    {
        currentCoffeeText.text = $"Café Restante: {amount}g";
    }
}
