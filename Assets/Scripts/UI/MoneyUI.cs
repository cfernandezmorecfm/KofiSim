using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currentMoneyText;

    private System.Action<MoneyChangedEvent> moneyChangedHandler;
    private void OnEnable() // Se llama cuando el objeto se activa
    {
        // Guardamos la referencia al handler para garantizar que Subscribe y Unsubscribe utilizan la misma
        moneyChangedHandler = UpdateMoneyUI; // No ponemos parentesis porque no queremos invocar el método, sino pasar la referencia

        // Suscripción al BUS
        EventBus.Subscribe(moneyChangedHandler);

        // La UI debe mostrar el saldo correcto desde el primer momento, por lo que inicializamos aquí el update
        UpdateMoneyUI(new MoneyChangedEvent(MoneyManager.Instance.GetMoney()));
    }

    private void OnDisable() // Se llama cuando el objeto se desactiva
    {
        // Hacemos Unsuscribe utilizando la misma referencia que en el OnEnable
        EventBus.Unsubscribe(moneyChangedHandler);
    }

    private void UpdateMoneyUI(MoneyChangedEvent evt)
    {
        currentMoneyText.text = $"{evt.NewAmount:F2} €";
    }

}
