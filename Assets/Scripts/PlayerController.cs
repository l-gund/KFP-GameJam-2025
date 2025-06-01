using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _moveSpeed = 5f;
    public float _attackSpeed = .5f
    ; public float _attackRange = 1.5f;
    public GameObject _slash;
    public LayerMask whoCanBeHit;
    public int damage = 5;

    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D _rb;
    private float _timePassed;
    private Collider2D[] _hits;
    private Vector2 _mouse_pos;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _timePassed = _attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY);
        moveDirection.Normalize();

        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
    }

    void FixedUpdate()
    {   
        _rb.velocity = new Vector2(moveDirection.x * _moveSpeed, moveDirection.y * _moveSpeed);
    }
}
