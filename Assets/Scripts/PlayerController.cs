using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10f;
    public Joystick joystick;

    private PlayerAnimationController animationController;
    private Rigidbody rb;
    Vector3 movement;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        // Handle player input
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
       // moveDirection = (-moveDirection);
        animationController.movementSpeed = moveDirection.magnitude;
        // Rotate player towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Move the player using Rigidbody velocity
        movement = moveDirection * moveSpeed;
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
