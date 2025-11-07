using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -10);

    void Update()
    {
        
    }

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
