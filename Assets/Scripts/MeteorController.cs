using System.Collections;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    /* Variable declaration */
    Rigidbody rb;
    public float floorHeight = 0.7f;
    
    /* Add a downwards force of -50.0f when the meteor is spawned */
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(0.0f, -50.0f, 0.0f);
    }

    /* If it hits a player when it falls, destroy itself and apply damage to the player, if it hits the ground, Expand() */
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().takeDamage(25);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(expand());
        }
    }

    /* Increases the size of the meteor rapidly for an explosion effect, resets the position to the floor height to avoid falling into the ground each size increase */
    private IEnumerator expand()
    {
        for (int i = 0; i < 100; i++)
        {
            gameObject.transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
            gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.003f);
        }
        Destroy(gameObject);
    }
}
