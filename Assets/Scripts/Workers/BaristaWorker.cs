using UnityEngine;

public class BaristaWorker : MonoBehaviour
{

    [SerializeField] private float preparationTime = 4f; // Tiempo que tarda el barista en preparar un pedido
    [SerializeField] private OrderQueue orderQueue;
    [SerializeField] private GameObject coffeePrefab;
    [SerializeField] private WorkStationCoffee workStation;

    [Header("Stats del trabajador")] // Para futuras mejoras, como habilidades o mejoras de salario, poder ver claro los atributos que pertenecen a las estadisticas del trabajador
    [SerializeField] private float salary = 40f; // Salario del barista por día

    public float Salary => salary; // Propiedad pública para acceder al salario desde otros scripts


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
        if (orderQueue.OrderCount == 0) return; // No hay pedidos en la cola, el barista espera

        // Comprobamos si hay suficiente café en stock para preparar el siguiente pedido
        if (!IngredientManager.Instance.TryUseCoffee(IngredientManager.Instance.CoffeGramsPerCup))
        {
            // No hay suficiente café para preparar el siguiente pedido
            return;
        }

        // Si hay pedidos en la cola y suficiente café, el barista comienza a preparar el siguiente pedido
        currentOrder = orderQueue.GetNextOrder();
        isPreparing = true;
        prepTimer = 0f;
        Debug.Log("Barista ha comenzado a preparar un pedido.");
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