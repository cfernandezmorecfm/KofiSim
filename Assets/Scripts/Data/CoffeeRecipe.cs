using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeRecipe", menuName = "KofiSim/CoffeeRecipe")]
public class CoffeeRecipe : ScriptableObject
{
    public float coffeePrice;
    public float gramsPerCup;
    public float preparationTime;
}
