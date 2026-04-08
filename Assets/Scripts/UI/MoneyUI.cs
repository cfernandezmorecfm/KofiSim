using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currentMoneyText;
    void Start()
    {
        // Suscribirse al evento de cambio de dinero
        MoneyManager.Instance.OnMoneyChanged += UpdateMoneyUI;

        // Actualizar la UI con el dinero inicial
        UpdateMoneyUI(MoneyManager.Instance.GetMoney());

    }

    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar errores al destruir el objeto
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.OnMoneyChanged -= UpdateMoneyUI;
        }
    }

    private void UpdateMoneyUI(float newMoney)
    {
        currentMoneyText.text = $"${newMoney:F2}";
    }

}
