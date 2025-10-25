using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Ammunition")]
    public GameObject fireBall;
    public float fireballSpeed = 10f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public int reserveAmmo = 90;

    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent<int, int> OnAmmoChanged;
    public UnityEvent OnPlayerDeath;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     if (UseAmmo())
        //     {
        //         ShootFireball();
        //     }
        // }
    }

    void ShootFireball()
    {
        if (fireBall != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 1f;
            Quaternion spawnRotation = transform.rotation;

            GameObject newFireball = Instantiate(fireBall, spawnPosition, spawnRotation);

            // Set the speed on the fireball's movement script
            FireballMovement movement = newFireball.GetComponent<FireballMovement>();
            if (movement != null)
            {
                movement.speed = fireballSpeed;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public bool UseAmmo(int amount = 1)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            OnAmmoChanged?.Invoke(currentAmmo, reserveAmmo);
            Debug.Log("shooting");
            return true;
        }
        return false;
    }

    public void Reload()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;
        OnAmmoChanged?.Invoke(currentAmmo, reserveAmmo);
    }
}