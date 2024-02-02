using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private FireBulletHellInArc bulletArc1;
    [SerializeField] private FireBulletHellInArc bulletArc2;
    private GameObject playerHitbox;
    private Light2D globalLight;
    public List<Vector2> positionList = new List<Vector2>();
    public float actionInterval;
    //How long the enemy is unable to do an action after shooting in seconds
    public float attackDowntime;
    public int phaseDifficulty;
    //So the enemy can't attack twice in a row or at the start of the battle
    public bool noAttack;
    [field: SerializeField]
    public UnityEvent<bool> OnAttack { get; set; }
    [field: SerializeField]
    public UnityEvent<float> AttackSpeed { get; set; }
    [field: SerializeField]
    public UnityEvent<bool> OnBlink { get; set; }
    void Start()
    {
        noAttack = true;
        playerHitbox = GameObject.FindWithTag("BulletHell");
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
        StartCoroutine(DoAction());
    }

    IEnumerator DoAction()
    {
        noAttack = true;
        int randomIndex = UnityEngine.Random.Range(0, positionList.Count);
        OnBlink.Invoke(true);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.transform.parent.gameObject.transform.position = positionList[randomIndex];
        OnBlink.Invoke(false);
        attackDowntime = 1.5f;
        yield return new WaitForSeconds(attackDowntime);
        noAttack = false;
        StartCoroutine(BulletHell(0));
    }

    IEnumerator BulletHell(int index)
    {
        playerHitbox.SetActive(true);
        globalLight.intensity = 0.2f;
        OnAttack.Invoke(true);
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
                break;
            case 1:
                break;
        }
        yield return new WaitForSeconds(attackDowntime);
        OnAttack.Invoke(false);
        bulletArc1.enabled = false;
        bulletArc2.enabled = false;
        attackDowntime = 2f;
        globalLight.intensity = 0.7f;
        playerHitbox.SetActive(false);
        StartCoroutine(DoAction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
