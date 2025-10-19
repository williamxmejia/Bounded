using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform cameraTarget;
    public float rotationSpeed = 300f;
    private float pitch = 0f;
    private float yaw = 0f;
    public float distance = 1000f;
    public float height = 2f;


    void Start()
    {
        Vector3 offset = new Vector3(0, height, -distance);
        transform.position = cameraTarget.position + offset;
        transform.LookAt(cameraTarget);

    }

    void Update()
    {
        AdjustRotation(3);

    }

    public void AdjustRotation(float speed)
    {
        float mouseX = Input.GetAxis("Mouse X") * speed;
        float mouseY = Input.GetAxis("Mouse Y") * speed;

        yaw += mouseX;
        // pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -40f, 80f);

        // Calculate the new rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate the new position
        Vector3 offset = rotation * new Vector3(0, 0, -distance) + new Vector3(0, height, 0);
        transform.position = cameraTarget.position + offset;

        // Always look at the target
        transform.LookAt(cameraTarget);

    }
}
