public readonly struct IngredientStockChangedEvent
{
    public float NewGrams { get; }

    public IngredientStockChangedEvent(float newGrams)
    {
        NewGrams = newGrams;
    }
}
