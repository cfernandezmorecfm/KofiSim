using UnityEngine;

public class CustomerFSM : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float patienceForTakeOrder = 20f; // Tiempo que el cliente está dispuesto a esperar para que le tomen su pedido antes de irse sin pagar
    [SerializeField] private float patienceForReceiveOrder = 30f; // Tiempo que el cliente está dispuesto a esperar para recibir su pedido antes de irse sin pagar

    private ICustomerState currentState;
    private Seat targetSeat;
    private Rigidbody2D rb;
    private float currentPatience;
    private OrderQueue orderQueue; // Referencia al sistema de pedidos para agregar el pedido del cliente
    private CustomerUI customerUI; // Referencia al componente de UI para actualizar la barra de paciencia

    public static int ActiveCount { get; private set; } = 0; // Contador estático para llevar la cuenta de los clientes activos en la escena

    // Propiedades públicas para que los estados accedan a los datos
    public float MoveSpeed => moveSpeed;
    public float PatienceForTakeOrder => patienceForTakeOrder;
    public float PatienceForReceiveOrder => patienceForReceiveOrder;
    public float CurrentPatience => currentPatience;
    public Seat TargetSeat => targetSeat;
    public Rigidbody2D Rb => rb;

    // Agregamos dos métodos para manejar el contador de clientes activos
    private void Awake()
    {
        ActiveCount++; // Incrementamos el contador cada vez que se crea un nuevo cliente
    }

    private void OnDestroy()
    {
        ActiveCount--; // Decrementamos el contador cada vez que un cliente es destruido
    }


    // Método público para encapsular la reducción de paciencia (lo llama el WaitingForServiceState)
    public void ReducePatience(float amount)
    {
        currentPatience -= amount;
    }
    void Start()
    {
        moveSpeed = Random.Range(1f, 4f); // Velocidad aleatoria para cada cliente
        rb = GetComponent<Rigidbody2D>();
        currentPatience = patienceForTakeOrder; // Inicialmente el cliente tiene paciencia para esperar a recibir su pedido
        FindFreeSeat();

        if (targetSeat != null)
        {
            ChangeState(new ArrivingState(this));
        }

        orderQueue = FindAnyObjectByType<OrderQueue>(); // Encuentra el sistema de pedidos en la escena

        customerUI = GetComponentInChildren<CustomerUI>(); // Encuentra el componente de UI en los hijos del cliente
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    //Creamos un método fixedUpdate para controlar el movimiento del cliente y evitar que se mueva a trompicones
    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedExecute();
        }
    }

    public void ChangeState(ICustomerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    private void FindFreeSeat()
    {
        Seat[] allSeats = FindObjectsByType<Seat>(FindObjectsSortMode.None);

        foreach (Seat seat in allSeats)
        {
            if (!seat.IsOccupied)
            {
                targetSeat = seat;
                targetSeat.Occupy(this);
                return;
            }
        }

        Debug.Log("No hay asientos libres, el cliente se va");
        Destroy(gameObject);
    }

    // Métodos públicos para que los estados puedan interactuar
    public void FreeSeat()
    {
        if (targetSeat != null)
        {
            targetSeat.Free();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    // Métodos para el sistema de pedidos (los llama el PlayerController)
    public bool CanTakeOrder()
    {
        return currentState is WaitingForServiceState;
    }

    public void StartOrdering()
    {
        if (currentState is WaitingForServiceState)
        {
            ChangeState(new OrderingState(this));
        }
    }

    public void CancelOrdering()
    {
        if (currentState is OrderingState)
        {
            ChangeState(new WaitingForServiceState(this));
        }
    }

    public void CompleteOrdering()
    {
        if (currentState is OrderingState)
        {
            orderQueue.AddOrder(this); // Agrega el pedido del cliente al sistema de pedidos
            currentPatience = patienceForReceiveOrder; // Reinicia la paciencia para esperar el pedido
            ChangeState(new WaitingForOrderState(this));
        }
    }

    public bool CanReceiveCoffee()
    {
        return currentState is WaitingForOrderState;
    }

    public void ReceiveCoffee()
    {
        if (currentState is WaitingForOrderState)
        {
            ChangeState(new ConsumingState(this));
        }
    }

    public CustomerUI UI { get { return customerUI; } }
}