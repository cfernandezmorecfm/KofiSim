using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 8f;

    private bool spawnEnabled = true; // Variable para controlar si el spawn está habilitado o no

    private float spawnTimer = 0f;
    private float nextSpawnTime;

    public void SetSpawningEnabled(bool isEnabled)
    {
        spawnEnabled = isEnabled;
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
        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Nuevo cliente ha llegado");
    }
}