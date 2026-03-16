using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.1f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Detectar el clic del mouse
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
            isMoving = true;
        }

        //Mover al jugador hacia la posición objetivo
        if (isMoving)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            float distance = direction.magnitude;

            //Detener al jugador si está cerca de la posición objetivo
            if (distance <= stoppingDistance)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
            else
            {
                rb.linearVelocity = direction.normalized * moveSpeed;
            }
        }
    }
}
