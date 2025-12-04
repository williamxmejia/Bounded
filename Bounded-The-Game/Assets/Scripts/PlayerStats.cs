using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Enemies")]
    public Enemies[] enemies;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
            {
                if (UseAmmo())
                {
                    ShootFireball();
                }
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Enemies hitEnemy = hit.collider.GetComponent<Enemies>();
                    if (hitEnemy != null && enemies != null)
                    {
                        foreach (Enemies enemy in enemies)
                        {
                            if (enemy == hitEnemy)
                            {
                                enemy.TakeDamage(10f);
                            }
                        }
                    }
                }
            }
        }
    }

    void ShootFireball()
    {
        if (fireBall != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 1f;
            Quaternion spawnRotation = transform.rotation;

            GameObject newFireball = Instantiate(fireBall, spawnPosition, spawnRotation);

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