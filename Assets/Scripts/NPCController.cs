using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private float interactRadius = 5;
    private PlayerController playerController;
    private bool runningInteraction = false;
    public GameObject interactText;
    bool playerInRange = false;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();    
        interactText.SetActive(false);
    }

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
        
        if (playerInRange && !runningInteraction && playerController.Interact())
        {
            StartCoroutine(Interaction());
        }
    }


    IEnumerator Interaction()
    {
        runningInteraction = true;
        interactText.SetActive(false);
        gameObject.SetActive(false);

        // Speak some dialog
        // give the player some exp
        // disappear
        runningInteraction = false;
        int expAmount = Random.Range(300, 1000);
        playerController.increaseExp(expAmount);
        Debug.Log("Player got: " + expAmount + " Exp!");
        yield return new WaitForSeconds(1);
    }
}
