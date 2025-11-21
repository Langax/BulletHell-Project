using System.Collections;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    Rigidbody rb;
    public float floorHeight = 0.7f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(0.0f, -50.0f, 0.0f);
    }

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
