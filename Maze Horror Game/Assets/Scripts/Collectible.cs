using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private MeshRenderer collectibleVisual;
    [SerializeField] private Light collectibleLight;
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
        collectibleVisual.enabled = false;
        collectibleLight.enabled = false;
        StartCoroutine(KillObject());
    }
    private IEnumerator KillObject() 
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource)) 
        {
            while (audioSource.volume > 0f)
            {
                audioSource.volume -= Time.deltaTime * 2;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
