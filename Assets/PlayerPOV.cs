using UnityEngine;

public class PlayerPOV : MonoBehaviour
{
    public Transform neck;
    public float sensitivity = 2.0f;
    public float minVertical = -90.0f;
    public float maxVertical = 90.0f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVertical, maxVertical);
        neck.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
