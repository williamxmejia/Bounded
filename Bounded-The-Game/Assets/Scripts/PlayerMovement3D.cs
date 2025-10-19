using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -19.62f;
    public float groundDistance = 0.4f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Camera playerCamera;
    
    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundMask;
    
    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    
    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        
        // If no player body is assigned, use this transform
        if (playerBody == null)
            playerBody = transform;
            
        // If no camera is assigned, try to find one
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        // Handle ground detection
        GroundCheck();
        
        // Handle player input
        HandleMovement();
        HandleMouseLook();
        HandleJumping();
        
        // Apply gravity
        ApplyGravity();
        
        // Move the player
        controller.Move(velocity * Time.deltaTime);
    }
    
    void GroundCheck()
    {
        // Check if player is grounded
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            // Fallback ground check using CharacterController
            isGrounded = controller.isGrounded;
        }
        
        // Reset velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
    }
    
    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys
        float vertical = Input.GetAxis("Vertical");     // W/S keys
        
        // Calculate movement direction relative to player rotation
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        
        // Move the player
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    
    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Rotate the camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Rotate the player body left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    void HandleJumping()
    {
        // Jump when space is pressed and player is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    void ApplyGravity()
    {
        // Apply gravity to velocity
        velocity.y += gravity * Time.deltaTime;
    }
    
    // Optional: Method to change movement speed (useful for power-ups, etc.)
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    // Optional: Method to make player jump (can be called from other scripts)
    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    // Optional: Get current movement state
    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    public bool IsMoving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f);
    }
}