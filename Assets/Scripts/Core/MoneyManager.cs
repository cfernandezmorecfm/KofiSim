using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private EconomyConfig economyConfig; // Acceso al SO de economía 
    [SerializeField] private Recipe recipe; // Acceso al SO de receta del café

    private float currentMoney; // Dinero actual del jugador

    private System.Action<CustomerPaidEvent> customerPaidHandler; 
    public float CoffeePrice => recipe.price; // Propiedad pública para acceder al precio del café desde otros scripts
    public float CurrentMoney => currentMoney; // Propiedad pública para acceder al dinero actual desde otros scripts
    private void Awake()
    {
        // Implementamos el patrón Singleton para asegurar que solo haya una instancia de MoneyManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruimos esta nueva
            return;
        }
        else
        {
            Instance = this; // Asignamos esta instancia como la única
            DontDestroyOnLoad(gameObject); // Opcional: hacemos que este objeto persista entre escenas
            currentMoney = economyConfig.StartingMoney; // Inicializamos el dinero actual con el valor de startingMoney para el dia 1
        }
    }

    private void OnEnable()
    {
        customerPaidHandler = OnCustomerPaid;
        EventBus.Subscribe(customerPaidHandler);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(customerPaidHandler);
    }

    private void OnCustomerPaid(CustomerPaidEvent evt)
    {
        AddMoney(evt.Amount);
    }
    private void AddMoney(float amount)
    {
        currentMoney += amount; // Sumamos el monto al dinero actual
        Debug.Log($"Dinero agregado: {amount}. Dinero actual: {currentMoney}"); // Log para verificar el cambio de dinero
        EventBus.Publish(new MoneyChangedEvent(currentMoney)); // Publicamos un evento para notificar a los suscriptores del cambio de dinero
    }

    public void SpendMoney (float amount)
    {
        currentMoney -= amount;
        Debug.Log($"Dinero gastado: -{amount:F2} | Total: {currentMoney:F2}");
        EventBus.Publish(new MoneyChangedEvent(currentMoney)); // Publicamos un evento para notificar a los suscriptores del cambio de dinero
    }
    public float GetMoney()
    {
        return currentMoney; // Retornamos el dinero actual
    }
}
