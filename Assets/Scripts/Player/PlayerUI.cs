using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] carrySlots; // Array de slots para mostrar los objetos que el jugador lleva

    public void UpdateCarriedItems(int count)
    {         // Activamos o desactivamos los slots según el número de objetos que el jugador lleva
        // El numero de objetos no puede ser mayor que el número de slots disponibles, que se marca en el inspector de Unity
        for (int i = 0; i < carrySlots.Length; i++)
        {
            carrySlots[i].SetActive(i < count);
        }
    }
}
