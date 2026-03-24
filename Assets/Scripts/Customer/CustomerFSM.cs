using UnityEngine;

public class CustomerFSM : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float patience = 15f;

    private ICustomerState currentState;
    private Seat targetSeat;
    private Rigidbody2D rb;
    private float currentPatience;

    // Propiedades públicas para que los estados accedan a los datos
    public float MoveSpeed { get { return moveSpeed; } }
    public float Patience { get { return patience; } }
    public float CurrentPatience { get { return currentPatience; } }
    public Seat TargetSeat { get { return targetSeat; } }
    public Rigidbody2D Rb { get { return rb; } }

    // Método público para encapsular la reducción de paciencia (lo llama el WaitingForServiceState)
    public void ReducePatience(float amount)
    {
        currentPatience -= amount;
    }
    void Start()
    {
        moveSpeed = Random.Range(3f, 8f); // Velocidad aleatoria para cada cliente
        rb = GetComponent<Rigidbody2D>();
        currentPatience = patience;
        FindFreeSeat();

        if (targetSeat != null)
        {
            ChangeState(new ArrivingState(this));
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
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
            ChangeState(new WaitingForOrderState(this));
        }
    }
}