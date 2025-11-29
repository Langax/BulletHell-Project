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
    private Animator animator;
    private AudioSource audioSource;
    private bool interactButtonPressed;
    private float cooldown, attackRange, swingCooldown;
    private int exp, level, expToNextLevel, health, maxHealth;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI interactText;
    public LevelUpButtonController option1, option2, option3;
    public bool selectingChoice = false;

    public Slider healthBar, expBar;
    public AudioClip swingSound;
    
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

        interactText.gameObject.SetActive(false);
        
        animator = GetComponent<Animator>();

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        expBar.maxValue = expToNextLevel;
        expBar.value = exp;

        audioSource = GetComponent<AudioSource>();
        SetText();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        movementDirection = (transform.forward * input.z + transform.right * input.x).normalized;
        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        transform.rotation = Quaternion.Euler(0.0f, cameraTransform.eulerAngles.y, 0.0f);

        expBar.maxValue = expToNextLevel;
        expBar.value = exp;
        if (exp >= expToNextLevel && !selectingChoice) 
        {
            LevelUp();
        }
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
            animator.SetTrigger("SwingTrigger");
            audioSource.PlayOneShot(swingSound, 1.0f);
            Swing();
            cooldown = swingCooldown;
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        interactButtonPressed = true;
    }

    public void IncreaseExp(int amount)
    {
        exp += amount;

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
        exp -= expToNextLevel;
        expToNextLevel  += 50;
        
        SetText();
        flipCards();
        
        maxHealth += 10;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        
        Debug.Log("Level: "  + level);
    }

    public void flipCards()
    {
        option1.flip();
        option2.flip();
        option3.flip();
        if (selectingChoice)
        {
            selectingChoice = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else
        {
            selectingChoice = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

    }
    
    private void SetText()
    {
        levelText.text = "Level: " + level;
    }

    public bool Interact()
    {
        if (interactButtonPressed)
        {
            interactButtonPressed = false;
            return true;
        }
        else
        {
            interactButtonPressed = false;
            return false;
        }
    }

    public void increaseMaxHP(int amount)
    {
        maxHealth += amount;
    }

    public void increaseAttackRange(float amount)
    {
        swingCooldown *= amount;
    }

    public void increaseAttackSpeed(float amount)
    {
        swingCooldown /= amount;
    }
}
