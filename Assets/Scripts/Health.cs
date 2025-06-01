using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    public void SetHealth(int amount)
    {
        health = amount;
    }

    public void SetMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    public void Damage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
