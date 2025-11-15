using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Variable Declaration */
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 input;
    private float cooldown = 0;
    private int exp, level, expToNextLevel, health, maxHealth;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    public TextMeshProUGUI levelText;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        exp = 0;
        level = 1;
        expToNextLevel = 100;
        health = 100;
        maxHealth = 100;

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

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector3>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (cooldown <= 0)
        {
            Swing();
            cooldown = 3f;
        }
    }

    public void IncreaseExp(int amount)
    {
        if (exp + amount > expToNextLevel)
        {
            LevelUp();
        }
        else
        {
            exp += amount;
        }
    }

    public void takeDamage(int amount)
    {
        if (health - amount <= 0)
        {
            //TODO: Game over
        }
        else
        {
            health -= amount;
        }
    }

    private void Swing()
    {
        float radius = 3f;
        Vector3 center = transform.position + transform.forward * 1.5f;

        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<BaseEnemyAI>().TakeDamage();
            }
        }
    }

    private void LevelUp()
    {
        level++;
        expToNextLevel  += 50;
        maxHealth += 10;
        health = maxHealth;
        SetText();
        Debug.Log("Level: "  + level);
    }

    private void SetText()
    {
        levelText.text = "Level: " + level;
    }
}
