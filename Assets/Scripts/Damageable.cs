using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator animator;
    public UnityEvent<int, int> healthChanged;

    [SerializeField]
    public int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health,MaxHealth);
            // nếu health = 0, monster sẽ chết
            if(_health <= 0 )
            {
                IsLive = false;
            }
        }
    }

    [SerializeField]
    private bool _isLive = true;

    [SerializeField]
    private bool isInvicible = false;

    private float timeSinceHit = 0;
    public float invicibilityTime = 0.25f;

    public bool IsLive
    {
        get { return _isLive; } 
        set
        {
            _isLive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }

    //khoá khả năng di chuyển của player hoặc monster nếu bị tác động vật lý
    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        private set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isInvicible)
        {
            if(timeSinceHit > invicibilityTime)
            {
                // remove invicibility
                isInvicible=false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsLive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;

            // notify other subscribed components that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }

        // không thể bị đánh
        return false;
    }
    public bool Heal(int healthRestore)
    {
        if (IsLive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealth(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
