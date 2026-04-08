public interface ICustomerState
{
    void Enter();
    void Execute();
    void FixedExecute() { } // El cliente se movía a trompicones, creamos un método nuevo para controlar el movimiento del cliente y que se ejecute en FixedUpdate() en el CustomerFSM
    void Exit();
}
