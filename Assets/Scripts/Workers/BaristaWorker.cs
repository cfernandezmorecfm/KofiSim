using UnityEngine;

public class BaristaWorker : MonoBehaviour
{

    [SerializeField] private float preparationTime = 4f; // Tiempo que tarda el barista en preparar un pedido
    [SerializeField] private OrderQueue orderQueue;
    [SerializeField] private WorkStationCoffee workStation;

    [Header("Stats del trabajador")] // Para futuras mejoras, como habilidades o mejoras de salario, poder ver claro los atributos que pertenecen a las estadisticas del trabajador
    [SerializeField] private float salary = 30f; // Salario del barista por día

    public float Salary => salary; // Propiedad pública para acceder al salario desde otros scripts


    private bool isPreparing = false;
    private float prepTimer = 0f;
    private CustomerFSM currentOrder;
    private int surplusCoffees = 0; // Variable para llevar la cuenta de los cafés sobrantes que se han preparado pero no se han recogido por los clientes, para corregir el bug de cafés sobrantes
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

        // Si hay un café sobrante registrado, lo usamos para preparar el siguiente pedido sin consumir más café del stock, y reducimos la cuenta de cafés sobrantes
        if (surplusCoffees > 0)
        {
            surplusCoffees--;
            orderQueue.GetNextOrder(); // Sacamos el siguiente pedido de la cola pero no lo asignamos a currentOrder porque no necesitamos hacer un seguimiento de él, ya que se prepara con café sobrante
            Debug.Log("Barista ha usado un café sobrante para preparar un pedido.");
            return;
        }

        // Comprobamos si hay suficiente café en stock para preparar el siguiente pedido
        if (!IngredientManager.Instance.TryUseCoffee(IngredientManager.Instance.CoffeGramsPerCup)) return;

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

        workStation.SpawnCoffee(); // El barista coloca el café preparado en el mostrador para que el cliente lo recoja

        currentOrder = null; // El pedido ha sido completado, se libera la referencia
    }

    public void RegisterSurplusCoffee()
    {
        surplusCoffees++;
        Debug.Log($"Café sobrante registrado. Total de cafés sobrantes: {surplusCoffees}");
    }

}