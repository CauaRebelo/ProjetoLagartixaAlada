using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject normalAttack;
    [SerializeField] private GameObject attackHitbox;
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

    //private TreeController treeController = TreeController.Instance;
    public bool[,] abilityTree = {{ false, false, false, false, false, false}, {false, false, false, false, false, false}, {false, false, false, false, false, false}, };

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

    public TMP_Text textoArvore;
    public int pontosArvore;
    //textoArvore.SetText("Pontos: " + pontosArvore.ToString());

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
        pontosArvore = 1;
        attackAnim = normalAttack.GetComponent<Animator>();
        textoArvore.SetText("Pontos Disponiveis: " + pontosArvore.ToString());
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
                if(abilityTree[0, 2])
                {
                    Instantiate(dashaoe_prefab_ice, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0 , 0), Quaternion.identity);
                    if (abilityTree[0, 3])
                    {
                        //Dash de Gelo 3
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 2])
                {
                    Instantiate(dashaoe_prefab_fire, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0, 0), Quaternion.identity);
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 3])
                    {
                        //Dash de Fogo 3
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 2])
                {
                    Instantiate(dashaoe_prefab_light, transform.position + new Vector3(0 + 1 * transform.localScale.x, 0, 0), Quaternion.identity);
                    if (abilityTree[2, 3])
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
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if(abilityTree[0, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 30;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 5;
                    if (abilityTree[0, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 40;
                        attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 6;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 4;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 5;
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 6;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Chainlight;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 15;
                    if (abilityTree[2, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 25;
                    }
                }
                break;
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
        Destroy(attackHitbox.GetComponent<StatusAttack>());
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
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if(abilityTree[0, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 25;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 5;
                    if (abilityTree[0, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 30;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 3;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 4;
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 4;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Chainlight;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 12;
                    if (abilityTree[2, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 20;
                    }
                }
                break;
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
        Destroy(attackHitbox.GetComponent<StatusAttack>());
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
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if(abilityTree[0, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 20;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 5;
                    if (abilityTree[0, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 25;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 4;
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 3;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Chainlight;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 10;
                    if (abilityTree[2, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 15;
                    }
                }
                break;
        }
        rb.velocity = Vector2.zero;
        OnAttack?.Invoke(false);
        OnLongAttack?.Invoke(false);
        normalAttack.SetActive(true);
        OnVerticalAttack?.Invoke(true);
        attackAnim.Play("VerticalSwingAir");
        yield return new WaitForSeconds(0.2f);
        if(hitEnemy)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower / 2);
            hitEnemy = false;
        }
        yield return new WaitForSeconds(verticalAttackAirCooldown - 0.2f);
        rb.drag = 0;
        normalAttack.SetActive(false);
        OnVerticalAttack?.Invoke(false);
        Destroy(attackHitbox.GetComponent<StatusAttack>());
        spamVerticalAttack = false;
        isAbleToAct = true;
        canMove = true;
        canAttack = true;
    }

    private void CheckforHittoJump()
    {
        while(spamVerticalAttack)
        {
            if(hitEnemy)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower / 2);
                hitEnemy = false;
                return;
            }
        }
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
        switch (enchantment)
        {
            case 0:
                break;
            case 1:
                if(abilityTree[0, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Slow;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 20;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 5;
                    if (abilityTree[0, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 25;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 4;
                    Debug.Log("Funcionou!");
                    if (abilityTree[1, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 3;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 4])
                {
                    attackHitbox.AddComponent<StatusAttack>();
                    attackHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Chainlight;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable1 = 2;
                    attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 10;
                    if (abilityTree[2, 5])
                    {
                        attackHitbox.GetComponent<StatusAttack>().effectVariable2 = 15;
                    }
                }
                break;
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
        Destroy(attackHitbox.GetComponent<StatusAttack>());
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
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable1 = 50;
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable2 = 10;
                    if (abilityTree[0, 1])
                    {
                        ripositeHitbox.GetComponent<StatusAttack>().effectVariable1 = 70;
                        ripositeHitbox.GetComponent<StatusAttack>().effectVariable2 = 12;
                    }
                }
                break;
            case 2:
                if (abilityTree[1, 0])
                {
                    //Reflect de Fogo 1
                    ripositeHitbox.AddComponent<StatusAttack>();
                    ripositeHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Burning;
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable1 = 5;
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable2 = 10;
                    if (abilityTree[1, 1])
                    {
                        ripositeHitbox.GetComponent<StatusAttack>().effectVariable1 = 10;
                    }
                }
                break;
            case 3:
                if (abilityTree[2, 0])
                {
                    //Reflect de Raio 1
                    ripositeHitbox.AddComponent<StatusAttack>();
                    ripositeHitbox.GetComponent<StatusAttack>().debuff = StatusEffects.DebuffsType.Chainlight;
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable1 = 1;
                    ripositeHitbox.GetComponent<StatusAttack>().effectVariable2 = 20;
                    if (abilityTree[2, 1])
                    {
                        ripositeHitbox.GetComponent<StatusAttack>().effectVariable2 = 50;
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

    public void TreePoint(int amount)
    {
        pontosArvore += amount;
        textoArvore.SetText("Pontos Disponiveis: " + pontosArvore.ToString());
    }

    public bool checkTree(int aux1, int aux2)
    {
        return abilityTree[aux1, aux2];
    }

    public void unlockSkill(int enchant)
    {
        if(pontosArvore > 0)
        {
            abilityTree[enchant/10, enchant%10] = true;
            Debug.Log(enchant%10);
            pontosArvore -= 1;
            textoArvore.SetText("Pontos Disponiveis: " + pontosArvore.ToString());
        }
    }
}
