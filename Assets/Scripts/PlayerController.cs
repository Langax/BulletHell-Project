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
    private Animator animator;
    private AudioSource audioSource; 
    private bool interactButtonPressed, selectingChoice;

    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI interactText;
    public Button[] buttons;
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

        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);
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
        if (exp + amount > expToNextLevel)
        {
            exp += amount;
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
        exp -= expToNextLevel;
        expToNextLevel  += 50;
        
        SetText();
        flipButtons();



        maxHealth += 10;
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        if (exp >= expToNextLevel)
        {
            Debug.Log("Player can level up again!");

            LevelUp();
        }
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
            selectingChoice = false;
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(false);
        }
        else
        {
            selectingChoice = true;
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

    public bool Interact()
    {
        interactText.gameObject.SetActive(true);
        if (interactButtonPressed)
        {
            interactText.gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}
