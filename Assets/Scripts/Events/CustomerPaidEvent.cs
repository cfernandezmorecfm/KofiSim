public readonly struct CustomerPaidEvent
{
    public float Amount { get; }
    public bool WasSatisfied { get; }

    public CustomerPaidEvent(float amount, bool wasSatisfied)
    {
        Amount = amount;
        WasSatisfied = wasSatisfied;
    }
}
