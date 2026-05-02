public readonly struct CustomerPaidEvent
{
    public float Amount { get; }

    public CustomerPaidEvent(float amount)
    {
        Amount = amount;
    }
}
