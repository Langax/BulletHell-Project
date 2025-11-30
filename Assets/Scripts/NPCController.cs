using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    /* Variable declaration */
    private float interactRadius = 5;
    private PlayerController playerController;
    private bool runningInteraction = false;
    public GameObject interactText;
    bool playerInRange = false;

    /* Initialize default values */
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();    
        interactText.SetActive(false);
    }

    /* Create two collision overlapSpheres, the inner one is the interaction radius, the outer one is to see if the player is near
        If the player is near, but not in range it'll deactivate the text, it's done this way to allow multiple NPC's to coexist and utilize the same InteractText */
    void Update()
    {
        playerInRange = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerInRange = true;
                interactText.SetActive(true);
                break;
            }
        }

        Collider[] hits2 = Physics.OverlapSphere(transform.position, interactRadius * 2);
        foreach (Collider hit in hits2)
        {
            if (hit.CompareTag("Player") && !playerInRange)
            {
                interactText.SetActive(false);
                break;
            }
        }
        
        /* If the player is in range, not already running an interaction and the F key is pressed, begin interaction coroutine */
        if (playerInRange && !runningInteraction && playerController.Interact())
        {
            StartCoroutine(Interaction());
        }
    }

    /* Deactivate the text and NPC, give the player a random amount of exp and then disappear */
    IEnumerator Interaction()
    {
        runningInteraction = true;
        interactText.SetActive(false);
        gameObject.SetActive(false);

        runningInteraction = false;
        int expAmount = Random.Range(300, 1000);
        playerController.increaseExp(expAmount);
        Debug.Log("Player got: " + expAmount + " Exp!");
        yield return new WaitForSeconds(1);
    }
}
