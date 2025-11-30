using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    /* Variable declaration */
    public Transform pivot;
    public Transform player;
    public Vector3 offset = new Vector3(0, 1, -8);

    private float turnDelta;
    private float sens = 5f;

    /* Rotate the camera pivot based on the mouseX input */
    void Update()
    {
        float mouseX = turnDelta * (sens * Time.deltaTime);
        pivot.Rotate(Vector3.up * mouseX);
    }
    
    /* Update the pivot position to be at around the same height as the player head, then update the camera position to be attached to the pivot + an offset
     LookAt pivot is used to keep the camera locked. Only left/right camera turning is possible */
    void LateUpdate()
    {
        pivot.position = player.position+ (new Vector3(0.0f, 0.5f, 0.0f));
        transform.position = pivot.position + (pivot.rotation * offset);
        transform.LookAt(pivot);

    }
    
    /* Public event function which gets the turnDelta from a mouse input */
    public void Turn(InputAction.CallbackContext context)
    {
        turnDelta = context.ReadValue<Vector2>().x;
    }
}
