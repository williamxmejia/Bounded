using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;
    private CharacterController controller;
    public float speed = 0.5f;
    public string playerTag = "Player";

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public HealthBar healthBar;

    private Transform player;

    void Start()
    {
        controller = GetComponent<CharacterController>();
  
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found. Make sure the player has the correct tag.");
        }

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        if (controller != null)
        {
            isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                controller.Move(direction * speed * Time.deltaTime);
            }


            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
 
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

            if (!isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0f;
            }

            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0f) currentHealth = 0f;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died: " + gameObject.name);
        Destroy(gameObject);
    }
}
