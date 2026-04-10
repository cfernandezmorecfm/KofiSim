

public interface IDayCycleState
{
    // Interfaz para los estados del ciclo del día, cada estado tendrá su propia implementación de esta interfaz
    void Enter();
    void Execute();
    void Exit();

}
