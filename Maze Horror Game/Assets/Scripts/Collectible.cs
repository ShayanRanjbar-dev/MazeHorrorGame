using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;
    private float rotateTime = 0f;
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        rotateTime += Time.deltaTime * rotateSpeed;
        float newYPosition = Mathf.Sin(rotateTime) / 2;
        transform.position = new(startPos.x, startPos.y + newYPosition, startPos.z);
    }
    public void DestroyCollectible() 
    {
        Destroy(gameObject);
    }
}
