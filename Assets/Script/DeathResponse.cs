using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathResponse : MonoBehaviour
{

    [SerializeField] private CanvasGroup myFade;

    public void Awake()
    {
        EventSystem.current.onDeath += OnDeath;
        EventSystem.current.onRespawn += OnRespawn;
    }

    private void OnDeath()
    {
        myFade.interactable = true;
        StartCoroutine(FadeIn());
    }

    private void OnRespawn()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        while(myFade.alpha < 1)
        {
            myFade.alpha += Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator FadeOut()
    {
        while (myFade.alpha > 0)
        {
            myFade.alpha -= Time.deltaTime;
            yield return new WaitForSeconds(0.005f);
        }
        myFade.interactable = false;
        myFade.gameObject.SetActive(false);
    }            
}
