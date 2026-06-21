using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private EconomyConfig economyConfig;

    // TRANSITORIO: referencias de café para los envoltorios que mantienen viva la tienda.
    [SerializeField] private IngredientSO coffeeIngredient;
    [SerializeField] private Recipe recipe;

    // Stock de cuánto tenemos de cada ingrediente.
    private readonly Dictionary<IngredientSO, float> stock = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Construimos el stock inicial a partir de la lista del EconomyConfig.
        foreach (IngredientAmount item in economyConfig.startingStock)
        {
            if (item.ingredient == null) continue;
            stock[item.ingredient] = item.amount;
        }
    }

    // === API GENÉRICA ===

    public float GetAmount(IngredientSO ingredient)
    {
        return stock.TryGetValue(ingredient, out float amount) ? amount : 0f;
    }

    public bool HasEnough(IngredientSO ingredient, float amount)
    {
        return GetAmount(ingredient) >= amount;
    }

    public void Add(IngredientSO ingredient, float amount)
    {
        stock[ingredient] = GetAmount(ingredient) + amount;
        PublishChange(ingredient);
    }

    public bool TryUse(IngredientSO ingredient, float amount)
    {
        if (!HasEnough(ingredient, amount)) return false;
        stock[ingredient] = GetAmount(ingredient) - amount;
        PublishChange(ingredient);
        return true;
    }

    // Consumo ATÓMICO de una lista (para recetas): comprueba que hay de TODOS antes de gastar nada.
    public bool HasEnoughForAll(List<IngredientAmount> items)
    {
        foreach (IngredientAmount item in items)
        {
            if (!HasEnough(item.ingredient, item.amount)) return false;
        }
        return true;
    }

    public bool TryUseAll(List<IngredientAmount> items)
    {
        if (!HasEnoughForAll(items)) return false;
        foreach (IngredientAmount item in items)
        {
            stock[item.ingredient] = GetAmount(item.ingredient) - item.amount;
            PublishChange(item.ingredient);
        }
        return true;
    }

    // Para que la UI pueda listar todos los ingredientes y sus cantidades.
    public IReadOnlyDictionary<IngredientSO, float> Stock => stock;

    private void PublishChange(IngredientSO ingredient)
    {
        EventBus.Publish(new IngredientStockChangedEvent(ingredient, GetAmount(ingredient)));
    }

    // ENVOLTORIOS TRANSITORIOS DE CAFÉ (mantienen viva la tienda hasta generalizarla) 

    public float CurrentCoffeGrams => GetAmount(coffeeIngredient);
    public float CoffeGramsPerCup => recipe.gramsPerCup;
    public bool TryUseCoffee(float grams) => TryUse(coffeeIngredient, grams);
    public void AddCoffee(float grams) => Add(coffeeIngredient, grams);
}
