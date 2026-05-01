using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance { get; private set; }

    [Header("Stock inicial")]
    [SerializeField] private float startingCoffeGrams = 1000f; // Empezamos con 1 kg de café gratis

    [Header("Consumo por café")]
    [SerializeField] private float coffeGramsPerCup = 20f;

    private float currentCoffeGrams;

    public float StartingCoffeGrams => startingCoffeGrams; // Para poder obtener la cantidad de gramos de café con la que empezamos de la instancia
    public float CurrentCoffeGrams => currentCoffeGrams; // Para poder obtener la cantidad de gramos de café que quedan de la instancia
    public float CoffeGramsPerCup => coffeGramsPerCup; // Para poder obtener la cantidad de gramos de café que se utilizan para una taza de la instancia

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentCoffeGrams = startingCoffeGrams; // Inicialización movida desde Start a Awake para asegurar que el stock de café se establezca correctamente al inicio del juego, incluso si el objeto se reinicia o se carga una nueva escena
    }

    public bool HasEnoughCoffee(float grams)
    {
        // Comprobamos si queda suficiente café en stock para poder preparar una taza
        return currentCoffeGrams >= grams;
    }

    public bool TryUseCoffee(float grams)
    {
        // El barista utiliza este método para intentar preparar una taza de café
        // Lo hacemos en un método separado para poder manejar el caso en el que no haya suficiente café y evitar que el barista prepare una taza sin café
        if (!HasEnoughCoffee(grams)) return false;
        currentCoffeGrams -= grams;
        EventBus.Publish(new IngredientStockChangedEvent(currentCoffeGrams)); // Publicamos el evento cada vez que se utiliza café para que el UI se actualice con el nuevo stock de café
        return true;
    }

    public void AddCoffee(float grams)
    {
        // Se utilizará en el menú de compra para agregar más café a nuestro stock
        currentCoffeGrams += grams;
        EventBus.Publish(new IngredientStockChangedEvent(currentCoffeGrams)); // Publicamos el evento cada vez que se agrega café para que el UI se actualice con el nuevo stock de café
    }
}
