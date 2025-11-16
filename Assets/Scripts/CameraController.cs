using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform pivot;
    public Transform player;
    public Vector3 offset = new Vector3(0, 1, -8);

    private float turnDelta;
    private float sens = 5f;

    void Update()
    {
        float mouseX = turnDelta * (sens * Time.deltaTime);
        pivot.Rotate(Vector3.up * mouseX);
    }
    
    void LateUpdate()
    {
        pivot.position = player.position+ (new Vector3(0.0f, 0.5f, 0.0f));
        transform.position = pivot.position + (pivot.rotation * offset);
        transform.LookAt(pivot);

    }
    
    public void Turn(InputAction.CallbackContext context)
    {
        turnDelta = context.ReadValue<Vector2>().x;
    }
}
