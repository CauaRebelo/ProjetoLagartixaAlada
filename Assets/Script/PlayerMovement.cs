using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject normalAttack;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerDamage playerDamage;

    public int enchantment;
    private int maxEnchantment;
    public float damage= 1f;

    private bool canMove = true;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;

    private Coroutine attackCo;
    private bool canAttack = true;
    public bool hitEnemy = false;
    private float attackCooldown = 0.3f;
    private float horizontalAttackCooldown = 0.5f;
    private float verticalAttackAirCooldown = 0.45f;
    private float verticalAttackCooldown = 0.5f;
    private Animator attackAnim;

    private bool canDash = true;
    public bool isAbleToAct = true;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.7f;

    [field: SerializeField]
    public UnityEvent<float> OnVelocityChange { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnGroundedChange { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnDash { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnAttack { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnLongAttack { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnVerticalAttack { get; set; }

    public void Start()
    {
        //EventSystem.current.onPlayerDamage += OnPlayerDamage;

        maxEnchantment = 4;
        attackAnim = normalAttack.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAbleToAct)
        {
            return;
        }

        OnGroundedChange?.Invoke(isGrounded());

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            if (canAttack == false)
            {
                normalAttack.SetActive(false);
                canAttack = true;
            }
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if(Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButton("Horizontal") && Input.GetButtonDown("Fire1") && canAttack)
        {
            attackCo = StartCoroutine(HorizontalAttack());
        }

        if (Input.GetButton("Vertical") && Input.GetButtonDown("Fire1") && isGrounded() && canAttack)
        {
            attackCo = StartCoroutine(VerticalAttack());
        }

        if (Input.GetButton("Vertical") && Input.GetButtonDown("Fire1") && canAttack)
        {
            attackCo = StartCoroutine(VerticalAttackAir());
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            attackCo = StartCoroutine(Attack());
        }

        if (Input.GetButtonDown("Fire3"))
        {
            enchantment = (enchantment + 1) % maxEnchantment;
            ChangeColor();
        }

        Flip(false);
    }

    private void FixedUpdate()
    {
        if (!isAbleToAct)
        {
            return;
        }

        if (canMove)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            OnVelocityChange?.Invoke(rb.velocity.x);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip(bool forced)
    {
        if((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) && (canMove || forced))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void ChangeColor()
    {
        switch (enchantment)
        {
            case 0:
                sprite.color = Color.white;
                break;
            case 1:
                sprite.color = new Color(0.5252461f, 0.6122692f, 0.7490196f);
                break;
            case 2:
                sprite.color = new Color(0.7490196f, 0.5252461f, 0.540786f);
                break;
            case 3:
                sprite.color = new Color(0.5438939f, 0.7490196f, 0.5252461f);
                break;
        }
    }

    private IEnumerator Dash()
    {
        Flip(true);
        canDash = false;
        isAbleToAct = false;
        playerDamage.iframe = true;
        if(canAttack == false)
        {
            normalAttack.SetActive(false);
            canAttack = true;
            StopCoroutine(attackCo);
        }
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        Physics2D.IgnoreLayerCollision(11, 15, true);
        OnDash?.Invoke(!canDash);
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        OnDash?.Invoke(canDash);
        playerDamage.iframe = false;
        trail.emitting = false;
        Physics2D.IgnoreLayerCollision(11, 15, false);
        rb.gravityScale = originalGravity;
        canMove = true;
        isAbleToAct = true;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator HorizontalAttack()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        normalAttack.SetActive(true);
        OnLongAttack?.Invoke(!canAttack);
        attackAnim.Play("HorizontalSwing");
        yield return new WaitForSeconds(horizontalAttackCooldown);
        rb.drag = 0;
        OnLongAttack?.Invoke(canAttack);
        normalAttack.SetActive(false);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator VerticalAttack()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        normalAttack.SetActive(true);
        OnVerticalAttack?.Invoke(!canAttack);
        attackAnim.Play("VerticalSwing");
        yield return new WaitForSeconds(verticalAttackCooldown);
        rb.drag = 0;
        OnVerticalAttack?.Invoke(canAttack);
        normalAttack.SetActive(false);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator VerticalAttackAir()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        if(hitEnemy)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower / 2);
            hitEnemy = false;
        }
        normalAttack.SetActive(true);
        OnVerticalAttack?.Invoke(!canAttack);
        attackAnim.Play("VerticalSwingAir");
        yield return new WaitForSeconds(verticalAttackAirCooldown);
        rb.drag = 0;
        normalAttack.SetActive(false);
        OnVerticalAttack?.Invoke(canAttack);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        normalAttack.SetActive(true);
        OnAttack?.Invoke(!canAttack);
        attackAnim.Play("NormalSwing");
        yield return new WaitForSeconds(attackCooldown);
        rb.drag = 0;
        OnAttack?.Invoke(canAttack);
        normalAttack.SetActive(false);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }
}
