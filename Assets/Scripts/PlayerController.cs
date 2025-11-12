using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Variable Declaration */
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 input;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movementDirection = (transform.forward * input.z + transform.right * input.x).normalized;
        rb.rotation = Quaternion.Euler(0.0f, cameraTransform.eulerAngles.y, 0.0f);
    }
    
    private void FixedUpdate()
    {
        Vector3 velocity = movementDirection * (movementSpeed * Time.deltaTime);
        rb.linearVelocity = new Vector3(velocity.x, 0.0f, velocity.z);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector3>();
    }
}
