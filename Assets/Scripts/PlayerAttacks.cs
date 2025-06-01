using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

    [SerializeField] private int slashDamage = 5;
    [SerializeField] private float slashSize = 1.5f;
    [SerializeField] private Transform slashPoint;
    [SerializeField] private float slashCooldown = .5f;

    [SerializeField] private float chargeCooldown = 2f;

    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float parryDuration = .25f;

    [SerializeField] private LayerMask whoCanBeHit;

    private float slashTimer;
    private float chargeTimer;
    private float parryTimer;
    private bool charging = false;
    private float timeCharging;
    // Start is called before the first frame update
    void Start()
    {
        slashTimer = slashCooldown;
        chargeTimer = chargeCooldown;
        parryTimer = parryCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && slashTimer > slashCooldown)
        {
            SlashAttack();
            slashTimer = 0f;
        }

        if (Input.GetMouseButton(1) && chargeTimer > chargeCooldown)
        {
            Debug.Log("Faggot Cunt Bitch Nigger Coon Kike Chink");
            chargeTimer = -999f;
            charging = true;
        }
        if (Input.GetMouseButtonUp(1) && charging)
        {
            
        }

        slashTimer += Time.deltaTime;
        chargeTimer += Time.deltaTime;
    }

    void SlashAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(slashPoint.transform.position, slashSize, whoCanBeHit);
        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().Damage(slashDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(slashPoint.transform.position, slashSize);
    }

    void ChargeAttack(float chargeTime)
    {
        float timePassed = 0;

        if (Input.GetMouseButtonUp(1) && timePassed > 0.1f)
        {
            
        }
    }
}
