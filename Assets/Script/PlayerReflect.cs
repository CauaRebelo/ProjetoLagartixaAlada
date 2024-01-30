using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReflect : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject ripositeAttack;
    [SerializeField] private PlayerDamage playerDamage;

    private Animator ripositeAnim;
    private float ripositeCooldown = 0.15f;
    private float iframeTime = 0.2f;

    void Start()
    {
        ripositeAnim = ripositeAttack.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Riposite()
    {
        playerMovement.OnParry?.Invoke(false);
        playerMovement.OnRiposite?.Invoke(true);
        ripositeAttack.SetActive(true);
        ripositeAnim.Play("Riposite");
        StartCoroutine(RipositeTime());
    }

    private IEnumerator RipositeTime()
    {
        playerDamage.parrying = false;
        playerDamage.iframe = true;
        yield return new WaitForSeconds(ripositeCooldown);
        playerMovement.OnRiposite?.Invoke(false);
        ripositeAttack.SetActive(false);
        playerMovement.ReflectEnd();
        yield return new WaitForSeconds(iframeTime);
        playerDamage.iframe = false;
    }
}
