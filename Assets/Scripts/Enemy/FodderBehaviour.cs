#nullable enable

using UnityEngine;

public class FodderBehaviour : MonoBehaviour
{
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

        animator!.SetFloat("velocityX", body!.velocity.x);
        animator!.SetFloat("velocityY", body!.velocity.y);

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
        Health? playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Damage(damage);
        }
    }
}