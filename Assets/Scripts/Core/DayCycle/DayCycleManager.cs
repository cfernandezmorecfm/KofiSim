using UnityEngine;
using System; 

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance { get; private set; }

    [Header("Configuración del ciclo del día")]
    [SerializeField] private float dayDurationInSeconds = 180f; // Duración de un día en segundos


    [Header("Referencias")]
    [SerializeField] private CustomerSpawner customerSpawner;
    [SerializeField] private BaristaWorker barista;

    private IDayCycleState currentState;
    private int currentDay = 1;

    // Datos económicos del día en curso (para el resumen de final de ciclo)
    private float dayIncome = 0f;
    private int coffeesServedToday = 0;

    private Action<CustomerPaidEvent> customerPaidHandler;
    private Action<CustomerServedEvent> customerServedHandler;

    //Conjuntos de expression-boided properties para exponer datos privados a otras clases sin permitir su modificación directa
    public int CoffeesServedToday => coffeesServedToday; // Para poder obtener la cantidad de cafés servidos desde la instancia
    public float DayDurationInSeconds => dayDurationInSeconds; // Para poder obtener la duración del día desde la instancia
    public float DayIncome => dayIncome;
    public int CurrentDay => currentDay;

    public CustomerSpawner CustomerSpawner => customerSpawner; // Para poder acceder al spawner de clientes desde los estados del ciclo del día
    public BaristaWorker Barista => barista; // Para poder acceder al barista desde los estados del ciclo del día

    public event Action<float> OnDayTimerChanged; // Evento para notificar a los suscriptores del cambio de tiempo del dia

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        customerPaidHandler = OnCustomerPaid;
        customerServedHandler = OnCustomerServed;
        EventBus.Subscribe(customerPaidHandler);
        EventBus.Subscribe(customerServedHandler);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(customerPaidHandler);
        EventBus.Unsubscribe(customerServedHandler);
    }
    private void Start()
    {
        ChangeState(new ServiceState(this));
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(IDayCycleState newState)
    {
        Debug.Log($"TRANSICIÓN: {currentState?.GetType().Name} → {newState.GetType().Name}");
        currentState?.Exit();
        StopAllCoroutines(); // Detener cualquier coroutine en ejecución (como el timer del día) al cambiar de estado
        currentState = newState;
        currentState?.Enter();
    }

    private void OnCustomerPaid(CustomerPaidEvent evt)
    {
        AddIncome(evt.Amount);
    }

    private void OnCustomerServed(CustomerServedEvent evt)
    {
        IncrementCoffeesServed();
    }
    public void AddIncome(float amount)
        {
            dayIncome += amount;
        }
    
    public void ResetDayIncome()
        {
            coffeesServedToday = 0;
            dayIncome = 0f;
        }
    
    public void AdvanceToNextDay()
    {
        currentDay++;
    }

    public void NotifyTimerChanged(float timeRemaining)
    {
        OnDayTimerChanged?.Invoke(timeRemaining);
    }

    // Método para incrementar la cantidad de cafés vendidos en el día actual, se llama desde el LeavingState cuando un cliente paga
    public void IncrementCoffeesServed()
    {
        coffeesServedToday++;
    }
}
