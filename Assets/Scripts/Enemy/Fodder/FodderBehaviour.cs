#nullable enable

using UnityEngine;

public class FodderBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string ANIMATOR_MAGNITUDE_PARAM = "magnitude";

    [SerializeField] private GameObject? player;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    private Animator? animator;
    private Rigidbody2D? body;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        body!.velocity = (player == null)
            ? Vector2.zero
            : (player.transform.position - transform.position).normalized * speed;

        animator!.SetFloat(ANIMATOR_MAGNITUDE_PARAM, body!.velocity.magnitude);

        UpdateScale();
    }

    private void UpdateScale()
    {
        if (body!.velocity.x == 0f)
        {
            return;
        }

        transform.localScale = new Vector3(
            (body!.velocity.x > 0f) ? 1f : -1f,
            transform.localScale.y
        );
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    void OnTriggerEnter2D(Collider2D collision)
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