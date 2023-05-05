using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(VaCham), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float tocDoThuong = 5f;
    public float tocDoChay = 8f;
    public float diBoTrenKhong = 3f;
    private float jumpImpulse = 10f;
    VaCham vacham;
    Damageable damageable;

    public GameObject gameOverButton;
    public GameObject gameOverText;

    public float CurrentMoveSpeed
    {
        get
        {
            if(CanMove)
            {
                if (IsMoving && !vacham.IsOnWall)
                {
                    if (vacham.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return tocDoChay;
                        }
                        else
                        {
                            return tocDoThuong;
                        }
                    }
                    else
                    {
                        // di chuyển trên không trung
                        return diBoTrenKhong;
                    }
                }
                else
                {
                    // đứng im
                    return 0;
                }
            }
            else
            {
                // khoá di chuyển
                return 0;
            }
        }
    }

    Vector2 nutDiChuyen;
    Rigidbody2D rb;
    Animator animator;

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // lấy ra giá trị true false của biến isAlive trong animator
    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vacham = GetComponent<VaCham>();
        damageable = GetComponent<Damageable>();
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _isRunning = false;
    
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set 
        { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set { 
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // tốc độ player
    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(nutDiChuyen.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    // hàm player di chuyển
    public void OnMove(InputAction.CallbackContext context)
    {
        nutDiChuyen = context.ReadValue<Vector2>();

        // nếu player còn sống thì vẫn di chuyển bình thường
        if(IsAlive)
        {
            IsMoving = nutDiChuyen != Vector2.zero;

            SetFacingDirection(nutDiChuyen);
        }
        // nếu player IsAlive = false => chết thì ko di chuyển được
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 nutDiChuyen)
    {
        if(nutDiChuyen.x > 0 && !IsFacingRight)
        {
            // quay mặt sang phải
            IsFacingRight = true;
        } else if(nutDiChuyen.x < 0 && IsFacingRight)
        {
            // quay mặt sang trái
            IsFacingRight = false;
        }
    }

    // hàm player chạy
    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        } else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    // hàm nhảy
    public void OnJump(InputAction.CallbackContext context) 
    {
        //
        if(context.started && vacham.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    // hàm tấn công
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "qua_man")
        {
            SceneManager.LoadScene("Level_2");
        }
        if(collision.gameObject.tag == "game_over")
        {
            SceneManager.LoadScene("StartMenu");

        }
    }
}
