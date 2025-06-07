using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBehavior : MonoBehaviour
{
    public float growthSpeed = .1f;
    public float attackDuration = 2f;
    public int shockwaveDamage = 25;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, attackDuration);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newSize = transform.localScale;
        newSize.x += growthSpeed;
        newSize.y += growthSpeed;
        transform.localScale = newSize;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit Something");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Something");
            other.GetComponent<Health>().Damage(shockwaveDamage);
        }
    }
}
