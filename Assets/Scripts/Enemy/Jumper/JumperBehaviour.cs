#nullable enable

using System.Collections.Generic;
using UnityEngine;

public class JumperBehaviour : MonoBehaviour
{
    private const float DIVER_SCALE = 1;
    private const string ANIMATOR_STATE_PARAM = "state";
    private const string ANIMATOR_MAGNITUDE_PARAM = "magnitude";

    [SerializeField] private GameObject? player;

    [SerializeField] private float jumpMaxDistance;
    [SerializeField] private float jumpMaxHeight;
    [SerializeField] private int jumpStartupTicks;
    [SerializeField] private int jumpActiveTicks;
    [SerializeField] private int jumpRecoveryTicks;

    private Animator? animator;
    private Rigidbody2D? body;
    private JumperStateManager? stateManager;
    private readonly DeferredActionManager deferredActionManager = new();

    private float jumpDistance;
    private Vector2 jumpStartPosition;
    private Vector2 jumpEndPosition;
    private int jumpCurrentTicks;

    void Start()
    {
        // animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        stateManager = GetComponent<JumperStateManager>();
    }

    void FixedUpdate()
    {
        deferredActionManager.OnFixedUpdate();

        JumperState state = stateManager!.GetState();
        switch (state)
        {
            case JumperState.Hostile:
                Jump();
                break;
            case JumperState.Jump:
                SetJumpArcPosition();
                break;
            default:
                break;
        }

        if (state == JumperState.Jump) jumpCurrentTicks++;

        UpdateScale();
    }

    private void Jump()
    {
        if (player == null)
        {
            return;
        }

        jumpDistance = Mathf.Min(
            Vector2.Distance(player.transform.position, transform.position),
            jumpMaxDistance
        );

        jumpStartPosition = transform.position;

        Vector3 jumpVector = (player.transform.position - transform.position).normalized * jumpDistance;
        jumpEndPosition = transform.position + jumpVector;
        jumpCurrentTicks = 0;

        UpdateState(JumperState.JumpStartup);
    }

    private void SetJumpArcPosition()
    {
        float time = (float)jumpCurrentTicks / jumpActiveTicks;
        float height = Mathf.Sin(Mathf.PI * time) * jumpMaxHeight;
        transform.position = Vector3.Lerp(jumpStartPosition, jumpEndPosition, time) + (Vector3.up * height);
    }

    private void UpdateVelocity(Vector2 velocity)
    {
        body!.velocity = velocity;
        // animator!.SetFloat(ANIMATOR_MAGNITUDE_PARAM, velocity.magnitude);
    }

    private void UpdateState(JumperState state)
    {
        switch (state)
        {
            case JumperState.JumpStartup:
                deferredActionManager.Defer(
                    jumpStartupTicks,
                    () => UpdateState(JumperState.Jump)
                );
                break;
            case JumperState.Jump:
                deferredActionManager.Defer(
                    jumpActiveTicks,
                    () =>
                    {
                        UpdateState(JumperState.JumpRecovery);
                        UpdateVelocity(Vector2.zero);
                    }
                );
                break;
            case JumperState.JumpRecovery:
                deferredActionManager.Defer(
                    jumpRecoveryTicks,
                    () => UpdateState(JumperState.Hostile)
                );
                break;
            default:
                break;
        }

        stateManager!.SetState(state);
        // animator!.SetInteger(ANIMATOR_STATE_PARAM, state.ToAnimatorState());
    }

    private void UpdateScale()
    {
        if (body!.velocity.x == 0f)
        {
            return;
        }

        transform.localScale = new Vector3(
            (body!.velocity.x > 0f) ? DIVER_SCALE : -DIVER_SCALE,
            transform.localScale.y
        );
    }
}