using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum PlayerState
{
    Walking,
    Parry,
    ChargingUp,
    ChargeForward
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Slash Details")]
    [SerializeField] private float slashFrequency = .5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private GameObject slash;
    [SerializeField] private LayerMask whoCanBeHit;
    [SerializeField] private int slashDamage = 5;
    private float slashTimer;


    [Header("Charge Details")]
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float chargeRate;
    [SerializeField] private float maxChargeDisatace;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int chargeDamage;
    private float chargeTimer;
    private Vector2 chargeEndPoint;
    private Vector2 chargepoint2;

    [Header("Parry Details")]
    [SerializeField] private float parryCooldown;
    [SerializeField] private float parryDuration;
    [SerializeField] private GameObject parryExplosion;
    private float parryTimer;



    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rb;
    private Collider2D[] hits;
    private Vector2 mousePos;
    private PlayerState state = PlayerState.Walking;
    private float parryCounter;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slashTimer = slashFrequency;
        parryTimer = parryCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.ChargingUp || state == PlayerState.Walking)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY);
            moveDirection.Normalize();
        }

        if (Input.GetMouseButtonDown(0) && slashTimer > slashFrequency)
        {
            Debug.Log("LMAO you suck");
            slashTimer = 0;
            SlashAttack();

        }

        if (Input.GetMouseButton(1) && chargeTimer > chargeCooldown)
        {
            Debug.Log("Charging up baby!");
            if (state != PlayerState.ChargingUp)
            {
                state = PlayerState.ChargingUp;
                chargeEndPoint = slash.transform.position;
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
            }
            if (Vector2.Distance(slash.transform.localPosition, transform.position) < maxChargeDisatace)
                slash.transform.localPosition = new Vector2(slash.transform.localPosition.x, slash.transform.localPosition.y + chargeRate);
        }
        else if (Input.GetMouseButtonUp(1) && state.Equals(PlayerState.ChargingUp))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            chargepoint2 = slash.transform.position;
            state = PlayerState.ChargeForward;
            slash.transform.position = chargeEndPoint;
            chargeTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && parryTimer > parryCooldown)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log("Starting to parry");
            state = PlayerState.Parry;
            parryCounter = parryDuration;
        }
        if (state == PlayerState.Parry)
        {
            SpriteRenderer ren = GetComponent<SpriteRenderer>();
            ren.material.color = Color.red;
            parryCounter -= Time.deltaTime;
            ParryAttack();
            if (parryCounter < 0 || state != PlayerState.Parry)
            {
                Debug.Log("End Parry");
                state = PlayerState.Walking;
                ren.material.color = Color.green;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }


        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

        slashTimer += Time.deltaTime;
        chargeTimer += Time.deltaTime;
        parryTimer += Time.deltaTime;

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Walking:
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
                break;
            case PlayerState.ChargeForward:
                Debug.Log("CHARGE MOTHER FUCKER!!!!!");
                Debug.Log(Vector2.Distance(transform.position, chargepoint2));
                ChargeAttack();
                rb.AddForce( new Vector2(chargepoint2.x - gameObject.transform.position.x, chargepoint2.y - gameObject.transform.position.y) * chargeSpeed, ForceMode2D.Impulse);
                if (Vector2.Distance(transform.position, chargepoint2) < 1f )
                    state = PlayerState.Walking;
                break;
            default:
                break;
        }
    }

    void SlashAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(slash.transform.position, attackRange, whoCanBeHit);
        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().Damage(slashDamage);
        }
    }

    void ChargeAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, whoCanBeHit);
        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().Damage(chargeDamage);
        }
    }

    void ParryAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, .6f, whoCanBeHit);
        if (hits.Length != 0)
        {
            Debug.Log("Activating Shockwave!!!!");
            Instantiate(parryExplosion, transform.position, Quaternion.identity);
            state = PlayerState.Walking;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(slash.transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, .6f);
    }
}
