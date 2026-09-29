using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;
    
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private WeaponSystem weaponSystem;
    
    private Vector3 velocity;
    private float xRotation = 0f;
    private float currentSpeed;
    private bool isGrounded;
    private float groundDragForce;
    
    private NetworkManager networkManager;
    
    private void Start()
    {
        networkManager = NetworkManager.Instance;
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleWeapon();
        ApplyPhysics();
    }
    
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        
        if (moveDirection.magnitude > 0.1f)
        {
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }
        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
        
        // Send movement to server
        networkManager.SendPlayerInput(transform.position, transform.eulerAngles, moveDirection * currentSpeed);
    }
    
    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        transform.Rotate(Vector3.up * mouseX * 2f);
        
        xRotation -= mouseY * 2f;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    private void HandleWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            weaponSystem.Fire();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponSystem.Reload();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            weaponSystem.SwitchWeapon();
        }
    }
    
    private void ApplyPhysics()
    {
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        groundDragForce = isGrounded ? groundDrag : airDrag;
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    public void TakeDamage(int damage)
    {
        // Handle taking damage
        Debug.Log("Player took " + damage + " damage");
    }
}
