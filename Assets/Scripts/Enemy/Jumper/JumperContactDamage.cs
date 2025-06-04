#nullable enable

using UnityEngine;

public class JumperContactDamage : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private int damage;

    // TODO: Bad practice for now, put state into a shared state manager later
    private JumperBehaviour? behaviour;

    void Start()
    {
        behaviour = GetComponent<JumperBehaviour>();
    }

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
            // Jumpers shouldn't be able to inflict damage while airborne
            Health? playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null && behaviour!.GetState() != JumperState.Jump)
            {
                playerHealth.Damage(damage);
            }
        }
    }
}