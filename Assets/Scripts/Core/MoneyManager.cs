using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    // Evento que se dispara cuando el dinero cambia
    // Cualquier script puede suscribirse a este evento para actualizar la UI u otras lógicas relacionadas
    public event Action<float> OnMoneyChanged; // Utilizamos system.Action para simplificar la declaración del evento
    [Header("Dinero inicial")]
    [SerializeField] private float startingMoney = 0f; // Dinero inicial del jugador

    [Header("Precio del café")]
    [SerializeField] private float coffeePrice = 2f; // Dinero que cuesta cada café

    private float currentMoney; // Dinero actual del jugador
                               
    public float CoffeePrice => coffeePrice; // Propiedad pública para acceder al precio del café desde otros scripts
    private void Awake()
    {
        // Implementamos el patrón Singleton para asegurar que solo haya una instancia de MoneyManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruimos esta nueva
            return;
        }
        else
        {
            Instance = this; // Asignamos esta instancia como la única
            DontDestroyOnLoad(gameObject); // Opcional: hacemos que este objeto persista entre escenas
            currentMoney = startingMoney; // Inicializamos el dinero actual con el valor de startingMoney para el dia 1
        }
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount; // Sumamos el monto al dinero actual
        Debug.Log($"Dinero agregado: {amount}. Dinero actual: {currentMoney}"); // Log para verificar el cambio de dinero
        OnMoneyChanged?.Invoke(currentMoney); // Disparamos el evento para notificar a los suscriptores del cambio
    }

    public void SpendMoney (float amount)
    {
        currentMoney -= amount;
        Debug.Log($"Dinero gastado: -{amount:F2} | Total: {currentMoney:F2}");
        OnMoneyChanged?.Invoke(currentMoney); // Disparamos el evento para notificar a los suscriptores del cambio
    }
    public float GetMoney()
    {
        return currentMoney; // Retornamos el dinero actual
    }
}
