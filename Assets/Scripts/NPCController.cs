using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private float interactRadius = 5;
    private PlayerController playerController;
    private bool runningInteraction = false;
    public GameObject interactText;
    
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();    
        interactText.SetActive(true);

    }

    void Update()
    {
        bool playerInRange = false;


        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerInRange = true;
                break;
            }
        }

        if (playerInRange && !runningInteraction && playerController.Interact())
        {
            StartCoroutine(Interaction());
        }
        
        if (playerInRange)
        {
            interactText.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
        }
    }


    IEnumerator Interaction()
    {
        runningInteraction = true;
        gameObject.SetActive(false);

        // Speak some dialog
        // give the player some exp
        // disappear
        runningInteraction = false;
        int expAmount = Random.Range(300, 1000);
        playerController.IncreaseExp(expAmount);
        Debug.Log("Player got: " + expAmount + " Exp!");
        yield return new WaitForSeconds(1);
        interactText.SetActive(false);
    }
}
