using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const int COLLISION_CLEAR_TICKS = 50;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    private HashSet<Collider2D> colliders = new();
    private List<DeferredAction> deferredActions = new();

    void FixedUpdate()
    {
        for (int i = deferredActions.Count - 1; i >= 0; i--)
        {
            DeferredAction action = deferredActions[i];
            if (action.ShouldExecute())
            {
                action.Execute();
                deferredActions.RemoveAt(i);
                continue;
            }

            action.Tick();
        }
    }

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

    void OnTriggerStay2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    private void HandleCollision(Collider2D collider)
    {
        if (colliders.Contains(collider))
        {
            return;
        }

        if (collider.TryGetComponent(out Damage damage) && damage.ShouldTrigger(gameObject))
        {
            damage.Trigger(gameObject);

            colliders.Add(collider);
            deferredActions.Add(
                new DeferredAction(
                    COLLISION_CLEAR_TICKS,
                    () => colliders.Remove(collider)
                )
            );
        }
    }
}
