#nullable enable

using System.Collections.Generic;
using UnityEngine;

public class ChargerBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string ANIMATOR_STATE_PARAM = "state";
    private const string ANIMATOR_MAGNITUDE_PARAM = "magnitude";

    private const float CHARGE_TRIGGER_DISTANCE = 2.5f;
    private const int CHARGE_STARTUP_TICKS = 60;
    private const int CHARGE_ACTIVE_TICKS = 60;
    private const int CHARGE_RECOVERY_TICKS = 180;
    private const int CHARGE_COOLDOWN = 300;

    [SerializeField] private GameObject? player;
    [SerializeField] private ChargerState state = ChargerState.Following;
    [SerializeField] private float speed;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int damage;

    private Animator? animator;
    private Rigidbody2D? body;
    private List<DeferredAction> deferredActions = new List<DeferredAction>();
    private int chargeCooldown = CHARGE_COOLDOWN;

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
            case ChargerState.Following:
                Follow();
                break;
            case ChargerState.PerformingCharge:
                Charge();
                break;
            default:
                break;
        }

        if (chargeCooldown > 0)
        {
            chargeCooldown--;
        }

        UpdateScale();
    }

    private void Follow()
    {
        if (player == null)
        {
            UpdateVelocity(Vector2.zero);
            return;
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (chargeCooldown == 0 && distance >= CHARGE_TRIGGER_DISTANCE)
        {
            UpdateVelocity(Vector2.zero);
            UpdateState(ChargerState.StartingCharge);
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
            UpdateState(ChargerState.Following);
            return;
        }

        Vector2 velocity = (player.transform.position - transform.position).normalized * chargeSpeed;
        UpdateVelocity(velocity);
        UpdateState(ChargerState.Charging);
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
            case ChargerState.StartingCharge:
                deferredActions.Add(
                    new DeferredAction(
                        CHARGE_STARTUP_TICKS,
                        () => UpdateState(ChargerState.PerformingCharge)
                    )
                );
                break;
            case ChargerState.Charging:
                deferredActions.Add(
                    new DeferredAction(
                        CHARGE_ACTIVE_TICKS,
                        () =>
                        {
                            UpdateState(ChargerState.RecoveringFromCharge);
                            UpdateVelocity(Vector2.zero);
                        }
                    )
                );
                break;
            case ChargerState.RecoveringFromCharge:
                deferredActions.Add(
                    new DeferredAction(
                        CHARGE_RECOVERY_TICKS,
                        () =>
                        {
                            UpdateState(ChargerState.Following);
                            chargeCooldown = CHARGE_COOLDOWN;
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