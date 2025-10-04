using System;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Player Refrences")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private FlashLight flashLight;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private AudioSource footstepAudio;

    [Header("Player Attributes")]
    [Header("Camera")]
    [SerializeField] private float cameraSensitivity = 0.2f;
    [SerializeField] private float lockRotation = 75f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 1f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float accelerateSpeed = 0.4f;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;

    private const float GROUND_VERTICAL_VELOCITY = -2f;
    private const float GRAVITY = -9.81f;

    private float currentSpeedMultiplier = 1f;
    private float verticalRotation = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    private bool isJumping = false;

    private float CurrentSpeed => walkSpeed * currentSpeedMultiplier;
    private ITrashObject currentObject;

    private float headBobTime = 0f;
    private float bobSpeed = 5f;
    private float bobAmount = 0.075f;
    private Vector3 startPos;
    private float footsteptime = 0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startPos = mainCamera.transform.localPosition;
        inputManager.OnPlayerJumpPressed += OnPlayerJumpPressed;
    }
    private void Update()
    {
        HandleRotation();
    }
    private void FixedUpdate()
    {   
        HandleVelocity();
    }
    private Vector3 CalculateWorldDirection() 
    {

        Vector3 inputDirection = new(inputManager.Move.x , 0 , inputManager.Move.y);
        Vector3 direction = transform.TransformDirection(inputDirection);
        currentSpeedMultiplier = 1f;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 75f, 0.2f);
        return direction.normalized;
    }
    private void CalculateVerticalVelocity() 
    {
        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                currentVelocity.y = jumpForce;
                isJumping = false;
            }
            else currentVelocity.y = GROUND_VERTICAL_VELOCITY;
        }
        else currentVelocity.y += GRAVITY * gravityMultiplier * Time.deltaTime;
    }
    private Vector3 CalculateSprinting() 
    {
        currentSpeedMultiplier = sprintSpeedMultiplier;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 85f, 0.2f);
        return transform.forward;
    }
    private void HandleVelocity() 
    {
        Vector3 worldDirection = (inputManager.IsSprinting)? CalculateSprinting() : CalculateWorldDirection();
        worldDirection *= CurrentSpeed;
        if (characterController.velocity.magnitude > 0.1f)
        {
            PlayFootstepAudio();
            if (characterController.isGrounded)
            {
                headBobTime += Time.deltaTime * bobSpeed * currentSpeedMultiplier;
                float posX = Mathf.Cos(headBobTime) * bobAmount;
                float posY = Mathf.Sin(headBobTime) * Mathf.Cos(headBobTime) * bobAmount * 1.5f;
                mainCamera.transform.localPosition = new Vector3(posX, startPos.y + posY, 0f);
            }
            else
            {
                mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, startPos, accelerateSpeed);
            }
        }
        else 
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition , startPos , accelerateSpeed);
        }
        currentVelocity.x = Mathf.Lerp(currentVelocity.x, worldDirection.x, accelerateSpeed);
        currentVelocity.z = Mathf.Lerp(currentVelocity.z, worldDirection.z, accelerateSpeed);
        CalculateVerticalVelocity();
        characterController.Move(currentVelocity * Time.deltaTime);
    }
    private void ApplyHorizontalRotation(float rotationAmount) => transform.Rotate(0, rotationAmount, 0);
    private void ApplyVerticalRotation(float rotationAmount) 
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -lockRotation , lockRotation);
       cameraHolder.transform.localRotation = Quaternion.Euler(verticalRotation, 0 , 0 );
    }
    private void HandleRotation()
    {
        float mouseXRotation = inputManager.Camera.x * cameraSensitivity;
        float mouseYRotation = inputManager.Camera.y * cameraSensitivity;
        flashLight.ApplyFlashlightRotation(mouseXRotation, mouseYRotation);
        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private void PlayFootstepAudio() 
    {
        if (characterController.isGrounded)
        {
            footsteptime += Time.deltaTime;
        }
        
        if (footsteptime >= 0.6f / currentSpeedMultiplier) 
        {
            footsteptime = 0;
            footstepAudio.pitch = UnityEngine.Random.Range(0.75f, 1.35f);
            footstepAudio.Play();
        }
    }
    private void OnPlayerJumpPressed()
    {
        if (characterController.isGrounded) isJumping = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        bool isCollectible = other.TryGetComponent<Collectible>(out Collectible collectible);
        if (isCollectible) 
        {
            collectible.DestroyCollectible();
            GameManager.Instance.AddScore();
        }

        bool isClown = other.TryGetComponent<EnemyAi>(out EnemyAi enemy);
        if (isClown)
        {
            GameManager.Instance.GameOver();
        }
    }
}
