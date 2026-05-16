
// Interfaz para los estados del ciclo del día, cada estado tendrá su propia implementación de esta interfaz
public interface IDayCycleState
{
    DayPhase Phase { get; } // Cada estado debe tener una fase asociada para identificarlo

    void Enter();
    void Execute();
    void Exit();

}
