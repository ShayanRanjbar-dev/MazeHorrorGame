using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float lockRotation = 15f;
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float returnSpeed = 2f;

    private Vector2 currentOffset;

    public void ApplyFlashlightRotation(float rotationX, float rotationY)
    {
        currentOffset.x += rotationX;
        currentOffset.y -= rotationY;
        currentOffset.x = Mathf.Clamp(currentOffset.x, -lockRotation, lockRotation);
        currentOffset.y = Mathf.Clamp(currentOffset.y, -lockRotation, lockRotation);
        Quaternion targetRotation = cameraHolder.rotation * Quaternion.Euler(currentOffset.y, currentOffset.x, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
        currentOffset = Vector2.Lerp(currentOffset, Vector2.zero, returnSpeed * Time.deltaTime);
    }
}