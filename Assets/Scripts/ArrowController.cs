using UnityEngine;
using UnityEngine.Animations;

public class ArrowController : MonoBehaviour
{
    private int archerDamage = 10;
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
