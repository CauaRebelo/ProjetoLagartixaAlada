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
    [SerializeField] private GameObject playerHitbox;
    [SerializeField] private GameObject ripositeHitbox;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerDamage playerDamage;

    public GameObject dashaoe_prefab_fire;
    public GameObject dashaoe_prefab_ice;
    public GameObject dashaoe_prefab_light;

    public int enchantment;
    private int maxEnchantment;
    public float damage= 1f;

    public bool[,] abilityTree = {{ false, false, false, false, false, false, false, false, false, false, false, false }, { false, false, false, false, false, false, false, false, false, false, false, false }, { false, false, false, false, false, false, false, false, false, false, false, false }, };

    public bool canMove = true;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 20f;
    public bool isFacingRight = true;

    private Coroutine attackCo;
    public bool canAttack = true;
    public bool hitEnemy = false;
    private bool spamAttack = false;
    private bool spamLongAttack = false;
    private bool spamVerticalAttack = false;
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

    private float parryStart = 0.3f;
    private float parryDuration = 0.2f;
    private float parryCooldown = 0.4f;

    [field: SerializeField]
    public UnityEvent<float> OnVelocityChange { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnGroundedChange { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnDash { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnParry { get; set; }

    [field: SerializeField]
    public UnityEvent<bool> OnRiposite { get; set; }

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

        if (Input.GetButton("Vertical") && Input.GetButtonDown("Fire2") && canAttack)
        {
            StartCoroutine(Reflect());
        }

        if (Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButton("Horizontal") && Input.GetButtonDown("Fire1") && canAttack && !spamLongAttack)
        {
            attackCo = StartCoroutine(HorizontalAttack());
        }

        if (Input.GetButton("Vertical") && Input.GetButtonDown("Fire1") && isGrounded() && canAttack && !spamVerticalAttack)
        {
            attackCo = StartCoroutine(VerticalAttack());
        }

        if (Input.GetButton("Vertical") && Input.GetButtonDown("Fire1") && canAttack && !spamVerticalAttack)
        {
            attackCo = StartCoroutine(VerticalAttackAir());
        }

        if (Input.GetButtonDown("Fire1") && canAttack && !spamAttack)
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
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if (abilityTree[0,4])
                {
                    //Dash de Gelo 1
                    playerHitbox.AddComponent<StatusAttack>();
                    playerHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    if (abilityTree[0, 6])
                    {
                        //Dash de Gelo 3
                    }
                    else if(abilityTree[0, 7])
                    {
                        //Dash de Gelo 4;
                    }
                }
                else if(abilityTree[0, 5])
                {
                    Instantiate(dashaoe_prefab_ice, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0 , 0), Quaternion.identity);
                    if (abilityTree[0, 6])
                    {
                        //Dash de Gelo 3
                    }
                    else if (abilityTree[0, 7])
                    {
                        //Dash de Gelo 4;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 4])
                {
                    //Dash de Fogo 1
                    playerHitbox.AddComponent<StatusAttack>();
                    playerHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 6])
                    {
                        //Dash de Fogo 3
                    }
                    else if (abilityTree[1, 7])
                    {
                        //Dash de Fogo 4;
                    }
                }
                else if (abilityTree[1, 5])
                {
                    Instantiate(dashaoe_prefab_fire, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0, 0), Quaternion.identity);
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 6])
                    {
                        //Dash de Fogo 3
                    }
                    else if (abilityTree[1, 7])
                    {
                        //Dash de Fogo 4;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 4])
                {
                    //Dash de Raio 1
                    if (abilityTree[2, 6])
                    {
                        //Dash de Raio 3
                    }
                    else if (abilityTree[2, 7])
                    {
                        //Dash de Raio 4;
                    }
                }
                else if (abilityTree[2, 5])
                {
                    Instantiate(dashaoe_prefab_light, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0, 0), Quaternion.identity);
                    if (abilityTree[2, 6])
                    {
                        //Dash de Raio 3
                    }
                    else if (abilityTree[2, 7])
                    {
                        //Dash de Raio 3
                    }
                }
                break;
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
        Destroy(playerHitbox.GetComponent<StatusAttack>());
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
        spamLongAttack = true;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        OnAttack?.Invoke(false);
        OnVerticalAttack?.Invoke(false);
        rb.velocity = Vector2.zero;
        normalAttack.SetActive(true);
        OnLongAttack?.Invoke(true);
        attackAnim.Play("HorizontalSwing");
        yield return new WaitForSeconds(horizontalAttackCooldown);
        rb.drag = 0;
        OnLongAttack?.Invoke(false);
        spamLongAttack = false;
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
        spamVerticalAttack = true;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        OnAttack?.Invoke(false);
        OnLongAttack?.Invoke(false);
        normalAttack.SetActive(true);
        OnVerticalAttack?.Invoke(true);
        attackAnim.Play("VerticalSwing");
        yield return new WaitForSeconds(verticalAttackCooldown);
        rb.drag = 0;
        OnVerticalAttack?.Invoke(false);
        normalAttack.SetActive(false);
        spamVerticalAttack = false;
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator VerticalAttackAir()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        spamVerticalAttack = true;
        hitEnemy = false;
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
        OnAttack?.Invoke(false);
        OnLongAttack?.Invoke(false);
        normalAttack.SetActive(true);
        OnVerticalAttack?.Invoke(true);
        attackAnim.Play("VerticalSwingAir");
        yield return new WaitForSeconds(verticalAttackAirCooldown);
        rb.drag = 0;
        normalAttack.SetActive(false);
        OnVerticalAttack?.Invoke(false);
        spamVerticalAttack = false;
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        isAbleToAct = false;
        canMove = false;
        spamAttack = true;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        rb.velocity = Vector2.zero;
        OnLongAttack?.Invoke(false);
        OnVerticalAttack?.Invoke(false);
        normalAttack.SetActive(true);
        OnAttack?.Invoke(true);
        attackAnim.Play("NormalSwing");
        yield return new WaitForSeconds(attackCooldown);
        rb.drag = 0;
        OnAttack?.Invoke(false);
        spamAttack = false;
        normalAttack.SetActive(false);
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private IEnumerator Reflect()
    {
        canAttack = false;
        canDash = false;
        isAbleToAct = false;
        canMove = false;
        if (canDash == false)
        {
            playerDamage.iframe = false;
        }
        OnParry?.Invoke(true);
        yield return new WaitForSeconds(parryStart);
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if (abilityTree[0, 0])
                {
                    //Reflect de Gelo 1
                    ripositeHitbox.AddComponent<StatusAttack>();
                    ripositeHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    if (abilityTree[0, 2])
                    {
                        //Reflect de Gelo 3
                    }
                    else if (abilityTree[0, 3])
                    {
                        //Reflect de Gelo 4;
                    }
                }
                else if (abilityTree[0, 1])
                {
                    //Reflect de Gelo 2
                    if (abilityTree[0, 2])
                    {
                        //Reflect de Gelo 3
                    }
                    else if (abilityTree[0, 3])
                    {
                        //Reflect de Gelo 4
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 0])
                {
                    //Reflect de Fogo 1
                    ripositeHitbox.AddComponent<StatusAttack>();
                    ripositeHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    if (abilityTree[1, 2])
                    {
                        //Reflect de Fogo 3
                    }
                    else if (abilityTree[1, 3])
                    {
                        //Reflect de Fogo 4;
                    }
                }
                else if (abilityTree[1, 1])
                {
                    //Reflect de Fogo 2
                    if (abilityTree[1, 2])
                    {
                        //Reflect de Fogo 3
                    }
                    else if (abilityTree[1, 3])
                    {
                        //Reflect de Fogo 4;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 0])
                {
                    //Reflect de Raio 1
                    if (abilityTree[2, 2])
                    {
                        //Reflect de Raio 3
                    }
                    else if (abilityTree[2, 3])
                    {
                        //Reflect de Raio 4;
                    }
                }
                else if (abilityTree[2, 1])
                {
                    //Reflect de Raio 2
                    if (abilityTree[2, 2])
                    {
                        //Reflect de Raio 3
                    }
                    else if (abilityTree[2, 3])
                    {
                        //Reflect de Raio 4;
                    }
                }
                break;
        }
        playerDamage.parrying = true;
        yield return new WaitForSeconds(parryDuration);
        playerDamage.parrying = false;
        yield return new WaitForSeconds(parryCooldown);
        rb.drag = 0;
        Destroy(ripositeHitbox.GetComponent<StatusAttack>());
        OnParry?.Invoke(false);
        canDash = true;
        canAttack = true;
        isAbleToAct = true;
        canMove = true;
    }

    public void unlockSkill(int enchant)
    {
        abilityTree[enchant/10, enchant%10] = true;
        Debug.Log(enchant%10);
    }
}
