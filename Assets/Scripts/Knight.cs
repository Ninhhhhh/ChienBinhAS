using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(VaCham), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public float tocDoDiChuyen = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone vungTanCong;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    VaCham vacham;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set 
        { 
            if(_walkDirection != value)
            {
                // đảo chiều di chuyển
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * (-1), gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.left;
                } else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.right;
                }
            }

            _walkDirection = value; 
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } 
        private set 
        { 
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vacham = GetComponent<VaCham>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    } 
    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        set { animator.SetFloat(AnimationStrings.attackCooldown,Mathf.Max(value,0)); }
    }
    // Update is called once per frame
    void Update()
    {
        HasTarget = vungTanCong.detectionColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(vacham.IsOnWall )
        {
            FlipDirection();
        }

        if(!damageable.LockVelocity)
        {
            if (CanMove)
                rb.velocity = new Vector2(tocDoDiChuyen * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        } 
        else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } 
        else
        {
            Debug.LogError("Hướng di chuyển không phải trái hoặc phải!!");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    public void OnCliffDetected()
    {
        if (vacham.IsGrounded)
        {
            FlipDirection();
        }
    }
}
