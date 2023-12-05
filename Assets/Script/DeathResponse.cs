using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathResponse : MonoBehaviour
{

    [SerializeField] private CanvasGroup myFade;

    public void Start()
    {
        EventSystem.current.onDeath += OnDeath;
    }

    private void OnDeath()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(myFade.alpha < 1)
        {
            myFade.alpha += Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(0.2f);
        while (myFade.alpha > 0)
        {
            myFade.alpha -= Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
    }
            
}
