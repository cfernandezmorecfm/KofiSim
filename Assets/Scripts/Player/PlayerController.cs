using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.05f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Seat"))
                {
                    //Ir a la posición x del taburete, manteniendo la posición y actual del jugador
                    targetPosition = new Vector2(hit.collider.transform.position.x, rb.position.y);
                    isMoving = true;
                }
                else if (hit.collider.CompareTag("WorkStation"))
                {
                    //Ir al centro del workstation de café
                    targetPosition = new Vector2(hit.collider.transform.position.x, rb.position.y);
                    isMoving = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            float distance = Vector2.Distance(rb.position, targetPosition);

            if (distance <= stoppingDistance)
            {
                rb.position = targetPosition;
                isMoving = false;
            }
            else
            {
                Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
        }
    }
}