using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 1f;
    public float gravity = -19.62f;
    public float groundDistance = 0.4f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Camera playerCamera;
    
    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundMask;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        
        if (playerBody == null)
            playerBody = transform;
            
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        GroundCheck();
        
        HandleMovement();
        HandleMouseLook();
        HandleJumping();
        
        ApplyGravity();
        
        controller.Move(velocity * Time.deltaTime);
    }
    
    void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {

            isGrounded = controller.isGrounded;
        }
        

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    void HandleMovement()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }
    
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
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