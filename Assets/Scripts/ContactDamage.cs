#nullable enable

using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            Health? playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
            }
        }
    }
}