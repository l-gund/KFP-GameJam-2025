using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerAttacks : MonoBehaviour
{

    [SerializeField] private int slashDamage = 5;
    [SerializeField] private float slashSize = 1.5f;
    [SerializeField] private Transform slashPoint;
    [SerializeField] private float slashCooldown = .5f;

    [SerializeField] private float chargeCooldown = 2f;
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float maxChargeDistance = 3f;
    [SerializeField] private float chargeRate = 1.1f;
 

    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float parryDuration = .25f;

    [SerializeField] private LayerMask whoCanBeHit;

    private float slashTimer;
    private float chargeTimer;
    private bool isCharging = false;
    private Vector2 slashStartPos;
    private float parryTimer;
    private float timeCharging;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        slashTimer = slashCooldown;
        chargeTimer = chargeCooldown;
        parryTimer = parryCooldown;
        rb = gameObject.GetComponent<Rigidbody2D>();
        slashStartPos = slashPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && slashTimer > slashCooldown)
        {
            SlashAttack();
            slashTimer = 0f;
        }

        if (Input.GetMouseButton(1))
        {
            /*
            isCharging = true;
            freeze gameObject.position
            if slash.magnitute > maxChargeDist
                slash.position++
            */
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 moveDirection = new Vector2(moveX, moveY);
            moveDirection.Normalize();

            isCharging = true;
            
            if (isCharging && slashPoint.position.magnitude > maxChargeDistance)
                slashPoint.position = new Vector2((slashPoint.position.x + moveX) * chargeRate, (slashPoint.position.y + moveY) * chargeRate);
                
            timeCharging = 0;
            timeCharging += Time.deltaTime;

        }
        else if (Input.GetMouseButtonUp(1) && isCharging)
        {
            ChargeAttack(timeCharging);
            isCharging = !isCharging;
            chargeTimer = 0f;
            gameObject.transform.position = gameObject.transform.position;
        }

        if (!isCharging)
            chargeTimer += Time.deltaTime;

        slashTimer += Time.deltaTime;
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
        Debug.Log("Charge!!!!!!");
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Horizontal"));
        rb.AddForce(transform.up * chargeSpeed, ForceMode2D.Impulse);
        Debug.Log(transform.up * chargeSpeed);
        slashPoint.position = slashStartPos;
    }
}
