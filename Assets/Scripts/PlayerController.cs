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
    private float cooldown, attackRange, swingCooldown, expBonus;
    private int exp, level, expToNextLevel, health, maxHealth;
    
    public int movementSpeed = 5;
    public Transform cameraTransform;
    public TextMeshProUGUI levelText;
    public LevelUpButtonController option1, option2, option3;
    public bool selectingChoice;
    public bool testMode = false;

    public Slider healthBar, expBar, swingBar;
    public AudioClip swingSound;
    
    /* Initialize default values */
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
        expBonus = 1f;
        
        animator = GetComponent<Animator>();

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        expBar.maxValue = expToNextLevel;
        expBar.value = exp;
 

        audioSource = GetComponent<AudioSource>();
        SetText();
        Cursor.lockState = CursorLockMode.Locked;


        if (testMode)
        {
            maxHealth = 10000;
            health = maxHealth;
            expBonus = 100;
        }
    }

    /* Update Swing cooldown & Exp bars, apply movement Animation & rotation (same as the camera rotation) */
    private void Update()
    {
        cooldown -= Time.deltaTime;
        swingBar.maxValue = swingCooldown;
        swingBar.value = cooldown;
        
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
        
        /* If they have enough exp to level up and aren't already in the level up menu, level up */
        if (exp >= expToNextLevel && !selectingChoice) 
        {
            LevelUp();
        }
    }
    
    /* Apply the movement as linearVelocity */
    private void FixedUpdate()
    {
        Vector3 velocity = movementDirection * (movementSpeed * Time.deltaTime);
        rb.linearVelocity = new Vector3(velocity.x, 0.0f, velocity.z);
    }

    /* Read the input value to determine direction */
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector3>();
    }

    /* Event function called on left click, if not already on cooldown, play the animation, audio clip and call the Swing function, then apply cooldown */
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

    /* Event function called on F input, sets a variable used later on so the NPC can determine whether the input was pressed whilst in range or not */
    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactButtonPressed = true;
        }
        else
        {
            interactButtonPressed = false;
        }
    }
    
    /* public function to return true or false depending on if the player recently hit the interact key, used inside the NPC controller */
    public bool Interact()
    {
        if (interactButtonPressed)
        {
            interactButtonPressed = false;
            return true;
        }

        interactButtonPressed = false;
        return false;
    }
    
    /* Public function to cause damage on the player, kill the player if their health will fall below 0 from the attack */
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

    /* Create an overlap sphere slightly in front of the player and cause damage on any enemies that were inside */
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

    /* Increment the level variable and increase the amount of exp for the next level, update the level text / Health bar & Flip the level up cards */
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
    }

    /* Flip each option and swap the selectingChoice variable, which will re-lock the cursor and set the time-scale back to normal*/
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
    
    /* Update level text */
    private void SetText()
    {
        levelText.text = "Level: " + level;
    }
    
    /* Public function to increase the max HP */
    public void increaseMaxHP(int amount)
    {
        maxHealth += amount;
    }

    /* Public function to increase the attack range */
    public void increaseAttackRange(float amount)
    {
        swingCooldown *= amount;
    }

    /* Public function to decrease the swing cooldown */
    public void increaseAttackSpeed(float amount)
    {
        swingCooldown *= amount;
    }

    /* Public function to increase the exp bonus */
    public void increaseExpBonus(float amount)
    {
        expBonus += amount;
    }
    
    /* Public function to increase the exp by a certain amount * expBonus */
    public void increaseExp(int amount)
    {
        exp += (int)(amount * expBonus);
    }
}
