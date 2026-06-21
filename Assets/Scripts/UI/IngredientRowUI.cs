using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Una fila del menú: icono + "Nombre: cantidad unidad".
public class IngredientRowUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI label;

    public void Set(IngredientSO ingredient, float amount)
    {
        if (iconImage != null)
        {
            iconImage.sprite = ingredient.icon;
            iconImage.enabled = ingredient.icon != null; // si no hay icono, se oculta
        }
        label.text = $"{ingredient.displayName}: {amount:0.#} {ingredient.unit}";
    }
}