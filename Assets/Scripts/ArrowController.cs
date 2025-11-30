using UnityEngine;
using UnityEngine.Animations;

public class ArrowController : MonoBehaviour
{
    /* Variable declaration, Archer damage needed here as this is where the collision check happens */
    private int archerDamage = 10;
    
    /* When the Arrow is spawned, face the player and apply a force towards it */
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player)
        {
            transform.LookAt(player.transform.position);
        }
        
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
    }

    /* If it hits the player, cause it damage and destroy itself, otherwise just destroy itself */
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().takeDamage(archerDamage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            
        }
    }
}
