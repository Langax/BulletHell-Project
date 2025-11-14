using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Variable Declaration */
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 input;
    private BoxCollider aoe;
    private float cooldown = 0;
    private int exp, level;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        aoe = GetComponent<BoxCollider>();
        aoe.enabled = false;
        exp = 0;
        level = 1;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        movementDirection = (transform.forward * input.z + transform.right * input.x).normalized;
        rb.rotation = Quaternion.Euler(0.0f, cameraTransform.eulerAngles.y, 0.0f);
    }
    
    private void FixedUpdate()
    {
        Vector3 velocity = movementDirection * (movementSpeed * Time.deltaTime);
        rb.linearVelocity = new Vector3(velocity.x, 0.0f, velocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            exp += other.gameObject.GetComponent<SkeletonMinionController>().expValue;
            Debug.Log("exp: " + exp);
            if (exp >= 100)
            {
                levelUp();
            }
            // other.takeDamage();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector3>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (cooldown <= 0)
        {
            StartCoroutine(swing(0.1f));
            cooldown = 2f;
        }
    }

    private IEnumerator swing(float seconds)
    {
        aoe.enabled = true;
        yield return new WaitForSeconds(seconds);
        aoe.enabled = false;
    }

    private void levelUp()
    {
        level++;
        Debug.Log("Level: "  + level);
    }
}
