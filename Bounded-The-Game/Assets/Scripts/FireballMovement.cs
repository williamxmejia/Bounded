using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    public string enemyTag = "Enemy";

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemies>() != null)
        {

            Enemies enemy = other.GetComponent<Enemies>();

            Destroy(other.gameObject);

            Destroy(gameObject);
        }
    }
}