using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float amplitude = 5f;
    public float speed = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        newY = Mathf.Max(newY, 0f);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
