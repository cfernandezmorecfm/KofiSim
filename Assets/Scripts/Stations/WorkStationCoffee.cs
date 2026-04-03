using UnityEngine;

public class WorkStationCoffee : MonoBehaviour
{

    //Creamos las variables para establecer la posición de creación de los cafés
    [SerializeField] private Transform coffeeSpawnPoint;
    [SerializeField] private float coffeeSpacing = 0.5f;

    //Agregamos una variable para llevar la cuenta de cuántos cafés se han creado
    private int coffeesOnCounter = 0;

    public Vector2 GetNextCoffeePosition()
    {
        Vector2 pos = new Vector2(
            coffeeSpawnPoint.position.x + (coffeesOnCounter * coffeeSpacing),
            coffeeSpawnPoint.position.y
        );
        coffeesOnCounter++; 
        return pos;
    }

    public void CoffeePickedUp()
    {
        if (coffeesOnCounter > 0)
        {
            coffeesOnCounter--;
        }
    }
}
