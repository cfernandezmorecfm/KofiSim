using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "KofiSim/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;     // Nombre visible
    public float price;     // Precio de venta de esta receta
    public float preparationTime; // Tiempo base de preparación (s)

    // Ingredientes que consume la receta. Cada entrada es (ingrediente, cantidad).
    public List<IngredientAmount> ingredients;

    // TRANSITORIO: lo siguen leyendo IngredientManager (CoffeGramsPerCup) y ShopPanelUI.
    // Se eliminará en el siguiente paso, cuando el stock pase a multi-ingrediente. No borrar aún.
    public float gramsPerCup;
}
