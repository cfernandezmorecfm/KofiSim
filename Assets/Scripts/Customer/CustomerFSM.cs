using UnityEngine;

public class CustomerFSM : MonoBehaviour
{
    public enum CustomerState
    {
        Arriving,
        WaitingForService,
        Ordering,
        WaitingForOrder,
        Consuming,
        Leaving
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float patience = 15f;

    private CustomerState currentState = CustomerState.Arriving;
    private Seat targetSeat;   
    private float currentPatience;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = Random.Range(4f, 7f);
        rb = GetComponent<Rigidbody2D>();
        currentPatience = patience;
        FindFreeSeat();
        Debug.Log("Probando el Log del Script del cliente");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CustomerState.Arriving:
                HandleArriving();
                break;
            case CustomerState.WaitingForService:
                HandleWaitingForService();
                break;
            case CustomerState.Ordering:
                HandleOrdering();
                break;
            case CustomerState.WaitingForOrder:
                HandleWaitingForOrder();
                break;
            case CustomerState.Consuming:
                HandleConsuming();
                break;
            case CustomerState.Leaving:
                HandleLeaving();
                break;
        }
    }

    private void FindFreeSeat()
    {
        // Buscar un asiento libre en la escena
        Seat[] allSeats = FindObjectsByType<Seat>(FindObjectsSortMode.None);
        
        foreach (Seat seat in allSeats)
        {
            if (!seat.IsOccupied)
            {
                Debug.Log("Cliente buscando asiento");
                targetSeat = seat;
                targetSeat.Occupy();
                return;
            }
        }
        // Si no hay asientos disponibles, el cliente se va
        Debug.Log("No hay asientos disponibles, el cliente se va");
        Destroy(gameObject);
    }

    private void HandleArriving()
    {
        // Creamos la lógica del estado de llegada
        Vector2 seatPos = new Vector2(targetSeat.Position.x, targetSeat.Position.y);
        float distance = Vector2.Distance(rb.position, seatPos);    

        if (distance <= 0.05f)
        {
            rb.position = seatPos;
            currentState = CustomerState.WaitingForService;
            //Agregamos Log para debuggear
            Debug.Log("Cliente sentado, esperando servicio.");
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, seatPos, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
        }
    }

    private void HandleWaitingForService()
    {
        // Creamos la lógica del estado de espera por servicio

        currentPatience -= Time.deltaTime;

        if (currentPatience <= 0f)
        {
            // El cliente se impacienta y se va, agregamos log para debuggear
            Debug.Log("Cliente se impacienta y se va");
            targetSeat.Free();
            Destroy(gameObject);
        }
    }

    private void HandleOrdering()
    {
        // Lógica para el estado de ordenando, se implementa más adelante
    }

    private void HandleWaitingForOrder()
    {
        // Lógica para el estado de esperando orden, se implementa más adelante
    }

    private void HandleConsuming()
    {
        // Lógica para el estado de consumiendo, se implementa más adelante
    }

    private void HandleLeaving()
    {
        // Lógica para el estado del cliente yendose, se implementa más adelante
    }
}
