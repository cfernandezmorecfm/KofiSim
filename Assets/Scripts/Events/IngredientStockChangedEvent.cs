public readonly struct IngredientStockChangedEvent
{
    public IngredientSO Ingredient { get; }
    public float NewAmount { get; }

    public IngredientStockChangedEvent(IngredientSO ingredient, float newAmount)
    {
        Ingredient = ingredient;
        NewAmount = newAmount;
    }
}