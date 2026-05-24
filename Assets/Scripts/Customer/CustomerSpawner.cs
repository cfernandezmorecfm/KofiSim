using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 8f;
    [SerializeField] private CustomerVisualProfile[] visualProfiles; // Array de perfiles visuales para asignar aleatoriamente a los clientes al spawnearlos

    private bool spawnEnabled = true; // Variable para controlar si el spawn está habilitado o no

    private float spawnTimer = 0f;
    private float nextSpawnTime;

    private int activeCustomers = 0;
    private Action<CustomerLeftEvent> customerLeftHandler;
    private Action<DayPhaseChangedEvent> dayPhaseChangedHandler;

    public int ActiveCustomers => activeCustomers;

    private void SetSpawningEnabled(bool isEnabled)
    {
        spawnEnabled = isEnabled;
    }

    private void OnEnable()
    {
        customerLeftHandler = OnCustomerLeft;
        EventBus.Subscribe(customerLeftHandler);

        dayPhaseChangedHandler = OnDayPhaseChanged;
        EventBus.Subscribe(dayPhaseChangedHandler);

        // PULL inicial para ponerse al día con la fase actual al activarse
        ApplyPhase(DayCycleManager.Instance.CurrentPhase);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(customerLeftHandler);
        EventBus.Unsubscribe(dayPhaseChangedHandler);
    }

    void Start()
    {
        //Asigna un tiempo de spawn aleatorio para el primer cliente
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        if (!spawnEnabled) return; // Si el spawn no está habilitado, no hacemos nada

        // Spawnea clientes a intervalos aleatorios
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnCustomer();
            spawnTimer = 0f;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void SpawnCustomer()
    {
        GameObject customerGO = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

        if (visualProfiles != null && visualProfiles.Length > 0)
        {
            CustomerVisualProfile randomProfile = visualProfiles[Random.Range(0, visualProfiles.Length)];
            CustomerFSM fsm = customerGO.GetComponent<CustomerFSM>();
            fsm.Initialize(randomProfile);
        }
        else
        {
            Debug.LogWarning("CustomerSpawner: el array visual está vacío");
        }

        activeCustomers++;
        EventBus.Publish(new CustomerSpawnedEvent());
        Debug.Log($"Nuevo cliente ha llegado. Clientes activos: {activeCustomers}");
    }

    private void OnCustomerLeft(CustomerLeftEvent evt)
    {
        activeCustomers--;
        Debug.Log($"Un cliente se ha ido. Clienes activos {activeCustomers}");
    }

    private void OnDayPhaseChanged(DayPhaseChangedEvent evt)
    {
        ApplyPhase(evt.NewPhase);
    }

    private void ApplyPhase(DayPhase phase)
    {
        if (phase == DayPhase.Service)
            SetSpawningEnabled(true);
        else if (phase == DayPhase.Closing)
            SetSpawningEnabled(false);
        // Para Summary y Shopping no hace faltahacer nada, porque el spawner ya viene de Closing con disabled
    }
}