using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private float interactRadius = 5;
    private PlayerController playerController;
    private bool runningInteraction = false;
    
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();    
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
                playerController.interactText.gameObject.SetActive(true);
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

        // Speak some dialog
        // give the player some exp
        // disappear
        runningInteraction = false;
        int expAmount = Random.Range(300, 1000);
        playerController.IncreaseExp(expAmount);
        Debug.Log("Player got: " + expAmount + " Exp!");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        playerController.interactText.gameObject.SetActive(false);
    }
}
