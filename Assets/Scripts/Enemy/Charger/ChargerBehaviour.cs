#nullable enable

using System.Collections.Generic;
using UnityEngine;

public class ChargerBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string ANIMATOR_STATE_PARAM = "state";
    private const string ANIMATOR_MAGNITUDE_PARAM = "magnitude";

    [SerializeField] private GameObject? player;
    [SerializeField] private ChargerState state = ChargerState.Hostile;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    [SerializeField] private float chargeTriggerDistance;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int chargeStartupTicks;
    [SerializeField] private int chargeActiveTicks;
    [SerializeField] private int chargeRecoveryTicks;
    [SerializeField] private int chargeCooldown;

    private Animator? animator;
    private Rigidbody2D? body;
    private List<DeferredAction> deferredActions = new();
    private int chargeCurrentCooldown = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        for (int i = deferredActions.Count - 1; i >= 0; i--)
        {
            DeferredAction action = deferredActions[i];
            if (action.GetTicks() == 0)
            {
                action.Execute();
                deferredActions.RemoveAt(i);
                continue;
            }

            action.Tick();
        }

        switch (state)
        {
            case ChargerState.Hostile:
                AttackPlayer();
                break;
            default:
                break;
        }

        if (chargeCurrentCooldown > 0) chargeCurrentCooldown--;
        UpdateScale();
    }

    private void AttackPlayer()
    {
        if (player == null)
        {
            UpdateVelocity(Vector2.zero);
            return;
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (chargeCurrentCooldown == 0 && distance >= chargeTriggerDistance)
        {
            UpdateVelocity(Vector2.zero);
            UpdateState(ChargerState.ChargeStartup);
            return;
        }

        UpdateVelocity(
            (player.transform.position - transform.position).normalized * speed
        );
    }

    private void Charge()
    {
        if (player == null)
        {
            UpdateState(ChargerState.Hostile);
            return;
        }

        Vector2 velocity = (player.transform.position - transform.position).normalized * chargeSpeed;
        UpdateVelocity(velocity);
        UpdateState(ChargerState.Charge);
    }

    private void UpdateVelocity(Vector2 velocity)
    {
        body!.velocity = velocity;
        animator!.SetFloat(ANIMATOR_MAGNITUDE_PARAM, velocity.magnitude);
    }

    private void UpdateState(ChargerState state)
    {
        switch (state)
        {
            case ChargerState.ChargeStartup:
                deferredActions.Add(
                    new DeferredAction(chargeStartupTicks, () => Charge())
                );
                break;
            case ChargerState.Charge:
                deferredActions.Add(
                    new DeferredAction(
                        chargeActiveTicks,
                        () =>
                        {
                            UpdateState(ChargerState.ChargeRecovery);
                            UpdateVelocity(Vector2.zero);
                        }
                    )
                );
                break;
            case ChargerState.ChargeRecovery:
                deferredActions.Add(
                    new DeferredAction(
                        chargeRecoveryTicks,
                        () =>
                        {
                            UpdateState(ChargerState.Hostile);
                            chargeCurrentCooldown = chargeCooldown;
                        }
                    )
                );
                break;
            default:
                break;
        }

        this.state = state;
        animator!.SetInteger(ANIMATOR_STATE_PARAM, state.ToAnimatorState());
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