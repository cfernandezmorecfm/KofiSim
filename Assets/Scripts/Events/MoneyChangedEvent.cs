public readonly struct MoneyChangedEvent
{
    public float NewAmount { get; }

    public MoneyChangedEvent(float newAmount)
    {
        NewAmount = newAmount;
    }
}