using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerAttacks : MonoBehaviour
{

    [Header("Slash Info")]
    [SerializeField] private int slashDamage = 5;
    [SerializeField] private float slashSize = 1.5f;
    [SerializeField] private Transform slashPoint;
    [SerializeField] private float slashCooldown = .5f;

    [Header("Charge Parameters")]
    [SerializeField] private float chargeCooldown = 2f;
    [SerializeField] private float chargeSpeed = .5f;
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

            slashStartPos = slashPoint.position;
            Vector2 moveDirection = new Vector2(moveX, moveY);
            moveDirection.Normalize();

            isCharging = true;

            if (isCharging && slashPoint.position.magnitude < maxChargeDistance)
                slashPoint.position = new Vector2((slashPoint.position.x + moveX), (slashPoint.position.y + moveY));

            timeCharging = 0;
            timeCharging += Time.deltaTime;
            Debug.Log(slashPoint.position);

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

        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Horizontal"));
        transform.position = Vector2.Lerp(transform.position, (new Vector2(slashPoint.position.x, slashPoint.position.y) + moveDir) * chargeSpeed, chargeSpeed);
        Debug.Log(transform.up * chargeSpeed);
        Debug.Log("end point:" + slashPoint.position + "  start point:" + slashStartPos);
        slashPoint.position = slashStartPos; 
        
    }
}
