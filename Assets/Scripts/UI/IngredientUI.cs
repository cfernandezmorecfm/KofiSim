using UnityEngine;
using TMPro;
public class IngredientUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentCoffeeText;
    void Start()
    {
        // Suscribirse al evento de cambio de stock de café
        IngredientManager.Instance.OnCoffeStockChanged += UpdateCoffeeText;

        // Actualizar la UI con el stock inicial de café
        UpdateCoffeeText(IngredientManager.Instance.StartingCoffeGrams);
    }

    private void OnDestroy()
    {
        if (IngredientManager.Instance != null)
        {
            IngredientManager.Instance.OnCoffeStockChanged -= UpdateCoffeeText;
        }
    }
    private void UpdateCoffeeText(float amount)
    {
        currentCoffeeText.text = $"Café Restante: {amount}g";
    }
}
