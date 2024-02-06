using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics.Tracing;
using System;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private FireBulletHellInArc bulletArc1;
    [SerializeField] private FireBulletHellInArc bulletArc2;
    [SerializeField] private FireBulletHellInSpiral bulletSpiral;
    [SerializeField] private GameObject protect;
    public GameObject playerHitbox;
    private Light2D globalLight;
    public List<Vector2> positionList = new List<Vector2>();
    public Vector2 above;
    public List<GameObject> attackPillarList = new List<GameObject>();
    public float actionInterval;
    //How long the enemy is unable to do an action after shooting in seconds
    public float attackDowntime;
    public int phaseDifficulty;
    //So the enemy can't attack twice in a row or at the start of the battle
    public bool noAttack;
    // função linear que começa em 90 e termina em 18 aos 20 segundos
    public float angularSpacing(float t) {
        Func<float,float,float,float,float,float> linear = (float aini, float afim, float tini, float tfim, float t) => aini + (afim-aini)*(t-tini)/(tfim-tini);
        if(t <= 4) return linear(45,30,0,4,t);
        else if(t <= 8) return linear(30,20,4,8,t);
        else if(t <= 12) return linear(20,18,8,12,t);
        else if(t <= 16) return linear(18,12,12,16,t);
        else return linear(12,1,16,20,t);
    }
    // tempo (em segundos) necessário pra dar uma volta de Fire
    public float firePeriod(float t) {
        if(t <= 4) return 2f;
        else if(t <= 8) return 1f;
        else if(t <= 12) return 0.8f;
        else if(t <= 16) return 0.5f;
        else return 0.1f;
    }
    public bool firstTime;
    [field: SerializeField]
    public UnityEvent<bool> OnAttack { get; set; }
    [field: SerializeField]
    public UnityEvent<float> AttackSpeed { get; set; }
    [field: SerializeField]
    public UnityEvent<bool> OnBlink { get; set; }
    void Start()
    {
        noAttack = true;
        //playerHitbox = GameObject.FindWithTag("BulletHell");
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
    }

    void OnEnable()
    {
        noAttack = true;
        firstTime = true;
        phaseDifficulty = 0;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        bulletArc1.enabled = false;
        bulletArc2.enabled = false;
        OnBlink.Invoke(false);
        OnAttack.Invoke(false);
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        StartCoroutine(DoAction());
    }

    IEnumerator DoAction()
    {
        int attack = UnityEngine.Random.Range(0, 2);
        if(phaseDifficulty == 2)
        {
            attack = 1;
        }
        int action = UnityEngine.Random.Range(0, 6);
        int randomIndex = UnityEngine.Random.Range(0, positionList.Count);
        OnBlink.Invoke(true);
        yield return new WaitForSeconds(0.4f);
        this.gameObject.transform.parent.gameObject.transform.position = positionList[randomIndex];
        OnBlink.Invoke(false);
        yield return new WaitForSeconds(0.2f);
        if(action != 0 && !noAttack)
        {
            if(attack == 1)
            {
                yield return new WaitForSeconds(1f);
                OnBlink.Invoke(true);
                yield return new WaitForSeconds(0.4f);
                this.gameObject.transform.parent.gameObject.transform.position = positionList[0];
                OnBlink.Invoke(false);
                yield return new WaitForSeconds(1f);
                attackDowntime = 1.5f;
                yield return new WaitForSeconds(attackDowntime);
                StartCoroutine(BulletHell());
            }
            else
            {
                attackDowntime = 1.5f;
                yield return new WaitForSeconds(attackDowntime);
                StartCoroutine(NormalAttack());
            }
        }
        else
        {
            noAttack = false;
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(DoAction());
        }
    }

    #region Attack Normal
    IEnumerator NormalAttack()
    {
        OnAttack.Invoke(true);
        int index = 0;
        int randomIndex;
        switch(phaseDifficulty)
        {
            case 0:
                index = UnityEngine.Random.Range(0, 2);
                break;
            case 1:
                index = 2;
                break;
        }
        switch(index)
        {
            case 0:
                randomIndex = UnityEngine.Random.Range(0, 2);
                if (randomIndex == 0)
                {
                    attackPillarList[0].SetActive(true);
                    attackPillarList[0].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    yield return new WaitForSeconds(0.4f);
                    attackPillarList[1].SetActive(true);
                    attackPillarList[1].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                }
                else
                {
                    attackPillarList[0].SetActive(true);
                    attackPillarList[0].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    yield return new WaitForSeconds(0.4f);
                    attackPillarList[2].SetActive(true);
                    attackPillarList[2].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                }
                break;
            case 1:
                randomIndex = UnityEngine.Random.Range(0, 3);
                if (randomIndex == 0)
                {
                    attackPillarList[3].SetActive(true);
                    attackPillarList[3].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);
                    attackPillarList[4].SetActive(true);
                    attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);
                    attackPillarList[5].SetActive(true);
                    attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);
                    attackPillarList[6].SetActive(true);
                    attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                }
                else if(randomIndex == 1)
                {
                    attackPillarList[7].SetActive(true);
                    attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);                    
                    attackPillarList[6].SetActive(true);
                    attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);                    
                    attackPillarList[5].SetActive(true);
                    attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.3f);                    
                    attackPillarList[4].SetActive(true);
                    attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                }
                else
                {                    
                    attackPillarList[3].SetActive(true);
                    attackPillarList[3].GetComponent<Animator>().Play("PillarAtaque");                    
                    attackPillarList[7].SetActive(true);
                    attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                    yield return new WaitForSeconds(0.4f);                    
                    attackPillarList[4].SetActive(true);
                    attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");                    
                    attackPillarList[6].SetActive(true);
                    attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                }

                break;
                case 2:
                randomIndex = UnityEngine.Random.Range(0, 3);
                if (randomIndex == 0)
                {
                    attackPillarList[0].SetActive(true);
                    attackPillarList[0].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    yield return new WaitForSeconds(0.4f);
                    attackPillarList[1].SetActive(true);
                    attackPillarList[1].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    randomIndex = UnityEngine.Random.Range(0, 2);
                    yield return new WaitForSeconds(0.5f);
                    if (randomIndex == 0)
                    {
                        attackPillarList[3].SetActive(true);
                        attackPillarList[3].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                    }
                    else
                    {
                        attackPillarList[7].SetActive(true);
                        attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    }
                }
                else if (randomIndex == 1)
                {
                    attackPillarList[0].SetActive(true);
                    attackPillarList[0].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    yield return new WaitForSeconds(0.4f);
                    attackPillarList[2].SetActive(true);
                    attackPillarList[2].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    randomIndex = UnityEngine.Random.Range(0, 3);
                    yield return new WaitForSeconds(0.5f);
                    if (randomIndex == 0)
                    {
                        attackPillarList[3].SetActive(true);
                        attackPillarList[3].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                    }
                    else if(randomIndex == 1)
                    {
                        attackPillarList[7].SetActive(true);
                        attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    }
                    else
                    {
                        attackPillarList[7].SetActive(true);
                        attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    }
                }
                else
                {
                    attackPillarList[1].SetActive(true);
                    attackPillarList[1].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    yield return new WaitForSeconds(0.4f);
                    attackPillarList[2].SetActive(true);
                    attackPillarList[2].GetComponent<Animator>().Play("PillarAtaqueHorizontal");
                    randomIndex = UnityEngine.Random.Range(0, 3);
                    yield return new WaitForSeconds(0.5f);
                    if (randomIndex == 0)
                    {
                        attackPillarList[3].SetActive(true);
                        attackPillarList[3].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                    }
                    else if(randomIndex == 1)
                    {
                        attackPillarList[7].SetActive(true);
                        attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    }
                    else
                    {
                        attackPillarList[7].SetActive(true);
                        attackPillarList[7].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[6].SetActive(true);
                        attackPillarList[6].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[5].SetActive(true);
                        attackPillarList[5].GetComponent<Animator>().Play("PillarAtaque");
                        yield return new WaitForSeconds(0.3f);                    
                        attackPillarList[4].SetActive(true);
                        attackPillarList[4].GetComponent<Animator>().Play("PillarAtaque");
                    }
                }
                break;
        }
        yield return new WaitForSeconds(attackDowntime);
        OnAttack.Invoke(false);
        attackDowntime = 2f;
        noAttack = true;
        StartCoroutine(DoAction());
    }
    #endregion

    #region Bullet Hell
    IEnumerator BulletHell()
    {
        playerHitbox.SetActive(true);
        globalLight.intensity = 0.2f;
        OnAttack.Invoke(true);
        attackDowntime = 2f;
        int index = 0;
        float attackDuration;
        switch(phaseDifficulty)
        {
            case 0:
                index = 0;
                break;
            case 1:
                index = 1;
                break;
            case 2:
                index = UnityEngine.Random.Range(2, 4);
                break;
        }
        attackDuration = 0f;
        switch(index)
        {
            case 0:
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 10f;
                bulletArc1.moveSpeed = 3f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 24;
                bulletArc1.startAngle = 0f;
                bulletArc1.endAngle = 360f;
                bulletArc1.enabled = true;
                attackDowntime = 5f;
                attackDuration = 0f;
                break;
            case 1:
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 3f;
                bulletArc1.moveSpeed = 3f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 24;
                bulletArc1.startAngle = 0f;
                bulletArc1.endAngle = 360f;
                bulletArc1.enabled = true;
                attackDowntime = 5f;
                bulletArc2.attackDelay = 1.5f;
                bulletArc2.attackInterval = 10f;
                bulletArc2.moveSpeed = 7f;
                bulletArc2.lifeTime = 15f;
                bulletArc2.bulletsAmount = 43;
                bulletArc2.startAngle = 0f;
                bulletArc2.endAngle = 360f;
                bulletArc2.enabled = true;
                attackDuration = 3f;
                break;
            case 2:
                bulletArc2.attackDelay = 0f;
                bulletArc2.attackInterval = 0.5f;
                bulletArc2.moveSpeed = 4f;
                bulletArc2.lifeTime = 15f;
                bulletArc2.bulletsAmount = 8;
                bulletArc2.startAngle = 225f;
                bulletArc2.endAngle = 315f;
                bulletArc2.enabled = true;
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 0.5f;
                bulletArc1.moveSpeed = 4f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 8;
                bulletArc1.startAngle = 45f;
                bulletArc1.endAngle = 135f;
                bulletArc1.enabled = true;
                yield return new WaitForSeconds(3f);
                bulletArc1.enabled = false;
                bulletArc2.enabled = false;
                yield return new WaitForSeconds(1f);
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 10f;
                bulletArc1.moveSpeed = 7f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 23;
                bulletArc1.startAngle = 0f;
                bulletArc1.endAngle = 360f;
                bulletArc1.enabled = true;
                yield return new WaitForSeconds(0.4f);
                bulletArc1.enabled = false;
                bulletArc2.attackDelay = 0f;
                bulletArc2.attackInterval = 0.5f;
                bulletArc2.moveSpeed = 4f;
                bulletArc2.lifeTime = 15f;
                bulletArc2.bulletsAmount = 8;
                bulletArc2.startAngle = 135f;
                bulletArc2.endAngle = 225f;
                bulletArc2.enabled = true;
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 0.5f;
                bulletArc1.moveSpeed = 4f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 8;
                bulletArc1.startAngle = 315f;
                bulletArc1.endAngle = 405f;
                bulletArc1.enabled = true;
                yield return new WaitForSeconds(3f);
                bulletArc1.enabled = false;
                bulletArc2.enabled = false;
                yield return new WaitForSeconds(1f);
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 10f;
                bulletArc1.moveSpeed = 7f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 23;
                bulletArc1.startAngle = 0f;
                bulletArc1.endAngle = 360f;
                bulletArc1.enabled = true;
                attackDowntime = 5f;
                attackDuration = 0f;
                break;
            case 3:
                OnBlink.Invoke(true);
                yield return new WaitForSeconds(0.4f);
                this.gameObject.transform.parent.gameObject.transform.position = above;
                OnBlink.Invoke(false);
                yield return new WaitForSeconds(1f);
                bulletArc1.attackDelay = 0f;
                bulletArc1.attackInterval = 1.2f;
                bulletArc1.moveSpeed = 3f;
                bulletArc1.lifeTime = 15f;
                bulletArc1.bulletsAmount = 22;
                bulletArc1.startAngle = 90f;
                bulletArc1.endAngle = 270f;
                bulletArc1.enabled = true;
                bulletArc2.attackDelay = 0.6f;
                bulletArc2.attackInterval = 1.2f;
                bulletArc2.moveSpeed = 3f;
                bulletArc2.lifeTime = 15f;
                bulletArc2.bulletsAmount = 25;
                bulletArc2.startAngle = 90f;
                bulletArc2.endAngle = 270f;
                bulletArc2.enabled = true;
                yield return new WaitForSeconds(4f);
                attackDowntime = 5f;
                attackDuration = 5f;
                break;
        }
        yield return new WaitForSeconds(attackDowntime);
        OnAttack.Invoke(false);
        bulletArc1.enabled = false;
        bulletArc2.enabled = false;
        yield return new WaitForSeconds(attackDuration);
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        noAttack = true;
        StartCoroutine(DoAction());
    }
    #endregion
    public void Damaged(bool val)
    {
        if(val)
        {
            this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            this.gameObject.GetComponent<Collider2D>().isTrigger = false;
            StopAllCoroutines();
            bulletArc1.enabled = false;
            bulletArc2.enabled = false;
        }
        else
        {
            DamagedStop();
        }
    }

    public void DamagedStop()
    {
        if(phaseDifficulty == 0)
        {
            phaseDifficulty++;
        }
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        
        StopAllCoroutines();
        bulletArc1.enabled = false;
        bulletArc2.enabled = false;
        StartCoroutine(DoAction());
    }

    public void Death(bool val)
    {
        if(val)
        {
            if(firstTime)
            {
                firstTime = false;
                StartCoroutine(FalseDeath());
                bulletArc1.enabled = false;
                bulletArc2.enabled = false;
                phaseDifficulty = 2;
            }
            else
            {
                StopAllCoroutines();
                bulletArc1.enabled = false;
                bulletArc2.enabled = false;
                StartCoroutine(Dying());
            }
        }
        else
        {
            DamagedStop();
        }
    }

    IEnumerator FalseDeath()
    {
        yield return new WaitForSeconds(1f);
        StopAllCoroutines();
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(DoAction());
    }

    IEnumerator Dying()
    {
        OnBlink.Invoke(true);
        yield return new WaitForSeconds(0.4f);
        this.gameObject.transform.parent.gameObject.transform.position = above;
        OnBlink.Invoke(false);
        yield return new WaitForSeconds(1f);
        playerHitbox.SetActive(true);
        globalLight.intensity = 0.2f;
        OnAttack.Invoke(true);
        yield return new WaitForSeconds(1f);
        protect.SetActive(true);
        float currentTime = 0f;
        float totalTime = 20f;
        float period;
        float angleEnd = angularSpacing(currentTime);
        float lastFireTime = currentTime;
        bulletSpiral.Fire(angleEnd);
        while(currentTime <= totalTime)
        {
            period = firePeriod(currentTime);
            angleEnd = angularSpacing(currentTime);
            if((currentTime - lastFireTime) >= period*(angleEnd/360))
            {
            lastFireTime = currentTime;
            bulletSpiral.Fire(angleEnd);
            }
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(4f);
        OnAttack.Invoke(false);
        bulletSpiral.enabled = false;
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        noAttack = true;
        EventSystem.current.BossDeath();
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void OnDisable()
    {
        StopAllCoroutines();
        noAttack = true;
        firstTime = true;
        phaseDifficulty = 0;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        bulletArc1.enabled = false;
        bulletArc2.enabled = false;
        OnBlink.Invoke(false);
        OnAttack.Invoke(false);
        protect.SetActive(false);
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
    }
}
