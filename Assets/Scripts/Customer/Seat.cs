using UnityEngine;

public class Seat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isOccupied = false;

    public bool IsOccupied
    {
        get { return isOccupied; }
    }
    
    public Vector2 Position
    {
        get { return transform.position; }
    }
    
    public void Occupy()
    {
        isOccupied = true;
    }

    public void Free()
    {
        isOccupied = false;
    }
}
