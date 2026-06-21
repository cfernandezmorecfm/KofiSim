using UnityEngine;

// Par (ingrediente + cantidad). [Serializable] para que Unity lo muestre dentro de
// listas en el Inspector. Reutilizable: lo usan las recetas (cantidad requerida)
// y más adelante el stock inicial (cantidad de partida).

[System.Serializable]
public struct IngredientAmount
{
    public IngredientSO ingredient; // Referencia al ScriptableObject del ingrediente
    public float amount; // Cantidad del ingrediente (en la unidad definida en el ScriptableObject)
}