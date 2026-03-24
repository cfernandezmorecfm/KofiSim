using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 8f;

    private float spawnTimer = 0f;
    private float nextSpawnTime;

    void Start()
    {
        //Asigna un tiempo de spawn aleatorio para el primer cliente
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
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