using UnityEngine;

// Define QUÉ es un ingrediente (no cuánto hay; eso lo lleva el IngredientManager).
// Añadir un ingrediente nuevo = crear un asset de este tipo. Cero código.

[CreateAssetMenu(fileName = "Ingredient", menuName = "KofiSim/Ingredient")]
public class  IngredientSO : ScriptableObject
{
    public string displayName; // Nombre del ingrediente que se mostrará en la UI
    public Sprite icon; // Icono del ingrediente que se mostrará en la UI
    public string unit; // Unidad de medida del ingrediente (por ejemplo, "g", "ml", "pcs")
}
