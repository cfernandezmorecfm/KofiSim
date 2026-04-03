using UnityEngine;

public class BaristaWorker : MonoBehaviour
{

    [SerializeField] private float preparationTime = 4f; // Tiempo que tarda el barista en preparar un pedido
    [SerializeField] private OrderQueue orderQueue;
    //[SerializeField] private Transform coffeSpawnPoint; Quitamos esta variable porque ahora la posición de creación del café se manejará desde la estación de trabajo
    [SerializeField] private GameObject coffeePrefab;
    [SerializeField] private WorkStationCoffee workStation;

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

        // Obtenemos la posición de creación del café desde la estación de trabajo
        Vector2 spawnPos = workStation.GetNextCoffeePosition();
        Instantiate(coffeePrefab, spawnPos, Quaternion.identity);

        currentOrder = null; // El pedido ha sido completado, se libera la referencia
    }

}