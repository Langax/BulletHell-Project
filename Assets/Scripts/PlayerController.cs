using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Variable Declaration */
    private Rigidbody rb;
    private Vector3 movementDirection;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = movementDirection * (movementSpeed * Time.deltaTime);
        rb.linearVelocity = new Vector3(velocity.x, 0.0f, velocity.z);

  
        rb.rotation = Quaternion.Euler(0.0f, cameraTransform.rotation.y, 0.0f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector3>();
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;

        movementDirection = (cameraForward * input.z + cameraRight * input.x).normalized;
    }
}
