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

    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float parryDuration = .25f;

    [SerializeField] private LayerMask whoCanBeHit;

    private float slashTimer;
    private float chargeTimer;
    private float parryTimer;
    private bool charging = false;
    private float timeCharging;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        slashTimer = slashCooldown;
        chargeTimer = chargeCooldown;
        parryTimer = parryCooldown;
        rb = gameObject.GetComponent<Rigidbody2D>();
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
            Debug.Log("Faggot Cunt Bitch Nigger Coon Kike Chink");
            charging = true;
            timeCharging = 0;
            timeCharging += Time.deltaTime;

        }
        else if (Input.GetMouseButtonUp(1) && charging)
        {
            ChargeAttack(timeCharging);
            charging = !charging;
            chargeTimer = 0f;
            gameObject.transform.position = gameObject.transform.position;
        }

        if (!charging)
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
        
    }
}
