using UnityEngine;

public class BaristaWorker : MonoBehaviour
{

    [SerializeField] private float preparationTime = 4f; // Tiempo que tarda el barista en preparar un pedido
    [SerializeField] private OrderQueue orderQueue;
    [SerializeField] private Transform coffeSpawnPoint;
    [SerializeField] private GameObject coffeePrefab;

    private bool isPreparing = false;
    private float prepTimer = 0f;
    private CustomerFSM currentOrder;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPreparing)
        {
            prepTimer += Time.deltaTime;

            if (prepTimer >= preparationTime)
            {
                FinishPreparation();
            }
        }
        else
        {
            TryStartNextOrder();
        }
    }

    private void TryStartNextOrder()
    {
        if (orderQueue.OrderCount > 0)
        {
            currentOrder = orderQueue.GetNextOrder();
            isPreparing = true;
            prepTimer = 0f;
            Debug.Log("Barista ha comenzado a preparar un pedido.");
        }
    }

    private void FinishPreparation()
    {
        isPreparing = false;
        Debug.Log("Barista ha terminado de preparar el pedido.");
        // Instanciar el café preparado en la posición del barista
        Instantiate(coffeePrefab, coffeSpawnPoint.position, Quaternion.identity);

        currentOrder = null; // El pedido ha sido completado, se libera la referencia
    }

}