public readonly struct DayPhaseChangedEvent
{
    public DayPhase NewPhase { get; }

    public DayPhaseChangedEvent(DayPhase newPhase)
    {
        NewPhase = newPhase;
    }
}
