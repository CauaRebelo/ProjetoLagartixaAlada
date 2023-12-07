using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool canAttack = true;
    private float attackCooldown = 0.3f;
    private Animator attackAnim;

    private bool canDash = true;
    public bool isAbleToAct = true;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.7f;

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

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            if (canAttack == false)
            {
                normalAttack.SetActive(false);
                canAttack = true;
            }
        }

        if(Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetButtonDown("Fire3"))
        {
            enchantment = (enchantment + 1) % maxEnchantment;
            ChangeColor();
        }

        Flip();
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
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) && canMove)
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
        canDash = false;
        isAbleToAct = false;
        playerDamage.iframe = true;
        if(canAttack == false)
        {
            normalAttack.SetActive(false);
            canAttack = true;
        }
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        Physics2D.IgnoreLayerCollision(11, 15, true);
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        playerDamage.iframe = false;
        trail.emitting = false;
        Physics2D.IgnoreLayerCollision(11, 15, false);
        rb.gravityScale = originalGravity;
        isAbleToAct = true;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
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
        attackAnim.Play("NormalSwing");
        yield return new WaitForSeconds(attackCooldown);
        normalAttack.SetActive(false);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }
}
