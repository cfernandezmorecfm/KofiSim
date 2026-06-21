using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EconomyConfig", menuName = "KofiSim/EconomyConfig")]
public class EconomyConfig : ScriptableObject
{
    public float StartingMoney;

    // Stock inicial de cada ingrediente al empezar la partida.
    public List<IngredientAmount> startingStock;

    // TRANSITORIO: ya no se usa (el stock inicial viene de startingStock). Eliminar más adelante.
    public float StartingCoffeeGrams;
}
