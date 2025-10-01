using UnityEngine;

public class GarbageTrashObject : MonoBehaviour , ITrashObject
{
    public void DeselectObject()
    {
        Debug.Log("deselect");
    }

    public void SelectObject()
    {
        Debug.Log("select");
    }
    public void InteractObject() 
    {
        Debug.Log("interact");
        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
