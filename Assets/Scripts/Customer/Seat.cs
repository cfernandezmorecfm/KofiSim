using UnityEngine;

public class Seat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isOccupied = false;
    private CustomerFSM currentCustomer;

    public bool IsOccupied
    {
        get { return isOccupied; }
    }
    
    public CustomerFSM CurrentCustomer
    {
        get { return currentCustomer; }
    }
    public Vector2 Position
    {
        get { return transform.position; }
    }
    
    public void Occupy(CustomerFSM customer)
    {
        isOccupied = true;
        currentCustomer = customer;
    }

    public void Free()
    {
        isOccupied = false;
        currentCustomer = null; 
    }
}
