using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float orderTakingSpeed = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private WorkStationCoffee coffeeStation;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private PlayerUI playerUI;

    // Creamos las variables de interacción con el cliente
    private Seat targetSeat;
    private CustomerFSM currentCustomer;
    private bool isTakingOrder = false;
    private float orderTimer = 0f;

    // Creamos las variables de interacción con la workstation de café
    private int carriedCoffees = 0; // Cantidad de café que el jugador está llevando
    private int maxCoffees = 2; // Cantidad máxima de cafés que el jugador puede llevar
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    void Update()
    {
        HandleInput();
        HandleOrderTaking();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero, Mathf.Infinity, interactableLayer);
            
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Seat"))
                {
                    //Si nos movemos mientras tomamos el pedido, se cancela la toma del pedido
                    CancelCurrentOrder();

                    targetPosition = new Vector2(hit.collider.transform.position.x, rb.position.y);
                    targetSeat = hit.collider.GetComponent<Seat>();
                    isMoving = true;
                }
                else if (hit.collider.CompareTag("WorkStation"))
                {
                    //Si nos movemos mientras tomamos el pedido, se cancela la toma del pedido
                    CancelCurrentOrder();

                    targetPosition = new Vector2(hit.collider.transform.position.x, rb.position.y);
                    targetSeat = null;
                    isMoving = true;   
                }
            }
        }
    }

    private void HandleMovement()
    {

        if (isMoving)
        {
            float distance = Vector2.Distance(rb.position, targetPosition);

            if (distance <= stoppingDistance)
            {
                rb.position = targetPosition;
                isMoving = false;

                // Para que no se intente tomar pedido y entregar café al mismo tiempo, se prioriza la entrega de café. Si no se entrega café, entonces se intenta tomar el pedido.
                if (!TryDeliverCoffee())
                {
                    TryStartOrder();
                }
                TryPickUpCoffee();

            }
            else
            {
                Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
        }
    }

    private void TryStartOrder()
    {
        if(targetSeat == null) return;
        if(!targetSeat.IsOccupied) return; 

        CustomerFSM customer = targetSeat.CurrentCustomer;
        if (customer != null && customer.CanTakeOrder())
        {
            currentCustomer = customer;
            currentCustomer.StartOrdering();
            isTakingOrder = true;
            orderTimer = 0f;
            Debug.Log("El jugador está tomando el pedido");
        }
    }

    private void HandleOrderTaking()
    {
        if(!isTakingOrder) return; 

        orderTimer += Time.deltaTime;

        if (orderTimer >= orderTakingSpeed)
        {
            currentCustomer.CompleteOrdering();
            Debug.Log("El jugador ha terminado de tomar el pedido");
            isTakingOrder = false;
            currentCustomer = null;
        }
    }

    private void CancelCurrentOrder()
    {
        if (isTakingOrder && currentCustomer != null)
        {
            currentCustomer.CancelOrdering();
            Debug.Log("El jugador ha cancelado la toma del pedido al moverse");
        }
        isTakingOrder = false;
        currentCustomer = null;
    }

    private void TryPickUpCoffee()
    {
        if (carriedCoffees >= maxCoffees) return;

        Collider2D[] coffees = Physics2D.OverlapCircleAll(rb.position, 1.5f);

        foreach (Collider2D col in coffees)
        {
            if (col.CompareTag("Coffee"))
            {
                Destroy(col.gameObject);
                carriedCoffees++;
                playerUI.UpdateCarriedItems(carriedCoffees); // Actualizamos la UI para mostrar el café que llevamos
                coffeeStation.CoffeePickedUp();
                Debug.Log("Jugador: he cogido un café. Llevo: " + carriedCoffees);

                if (carriedCoffees >= maxCoffees) break;
            }
        }
    }

    private bool TryDeliverCoffee()
    {
        if (carriedCoffees <= 0) return false;
        if (targetSeat == null) return false;
        if (!targetSeat.IsOccupied) return false;

        CustomerFSM customer = targetSeat.CurrentCustomer;
        if (customer != null && customer.CanReceiveCoffee())
        {
            customer.ReceiveCoffee();
            carriedCoffees--;
            playerUI.UpdateCarriedItems(carriedCoffees); // Actualizamos la UI para mostrar el café que llevamos
            Debug.Log("Jugador: café entregado. Me quedan: " + carriedCoffees);
            return true;
        }
        return false;
    }

}

