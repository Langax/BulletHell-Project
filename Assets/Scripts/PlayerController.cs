using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /* Variable Declaration */
    private Rigidbody rb;
    private Vector3 movementDirection;
    private Vector3 input;
    private float cooldown, attackRange, swingCooldown;
    private int exp, level, expToNextLevel, health, maxHealth;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    public TextMeshProUGUI levelText;
    public Button[] buttons;
    public Slider healthBar, expBar;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        exp = 0;
        level = 1;
        expToNextLevel = 100;
        maxHealth = 100;
        health = maxHealth;
        attackRange = 3f;
        swingCooldown = 3f;

        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        expBar.maxValue = expToNextLevel;
        expBar.value = exp;

        SetText();
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
            cooldown = swingCooldown;
        }
    }

    public void IncreaseExp(int amount)
    {
        if (exp + amount > expToNextLevel)
        {
            LevelUp();
            expBar.maxValue = expToNextLevel;
            expBar.value = exp;
        }
        else
        {
            exp += amount;
            expBar.maxValue = expToNextLevel;
            expBar.value = exp;
        }
    }

    public void takeDamage(int amount)
    {
        if (health - amount <= 0)
        {
            Destroy(gameObject);
            //TODO: Game over
        }
        else
        {
            health -= amount;
            healthBar.value = health;
        }
    }

    private void Swing()
    {
        float radius = attackRange;
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
        exp = 0;
        maxHealth += 10;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        SetText();

        flipButtons();
        Debug.Log("Level: "  + level);
    }

    private void SetText()
    {
        levelText.text = "Level: " + level;
    }

    private void flipButtons()
    {
        Time.timeScale = 0;
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        
        if (buttons[0].isActiveAndEnabled)
        {
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(false);
        }
        else
        {
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
            buttons[2].gameObject.SetActive(true);
        }
    }

    public void increaseAttackRange()
    {
        attackRange *= 1.1f;
        flipButtons();
        Time.timeScale = 1;
    }

    public void increaseMaxHealth()
    {
        maxHealth += 20;
        healthBar.maxValue = maxHealth;

        flipButtons();
        Time.timeScale = 1;
    }

    public void increaseAttackSpeed()
    {
        swingCooldown *= 0.8f;
        flipButtons();
        Time.timeScale = 1;
    }
}
