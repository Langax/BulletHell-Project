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
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && !runningInteraction)
            {
                StartCoroutine(Interaction());
            }
        }
    }

    IEnumerator Interaction()
    {
        runningInteraction = true;
        bool interactCheck = playerController.Interact();
        
        if (interactCheck)
        {
            // Speak some dialog
            // give the player some exp
            // disappear
            int expAmount = Random.Range(0, 1000);
            playerController.IncreaseExp(expAmount);
            Debug.Log("Player got: " + expAmount + " Exp!");
            yield return new WaitForSeconds(1);
            runningInteraction = false;
            gameObject.SetActive(false);
        }
        else
        {
            runningInteraction = false;
        }
    }
}
