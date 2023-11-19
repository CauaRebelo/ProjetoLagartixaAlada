using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{

    private float immuneTime = 1f;

    void Damage()
    {
        if(!Info_Player.iframe)
        {
            Info_Player.health--;
            EventSystem.current.PlayerDamage();
            StartCoroutine(Immune());
        }

        if(Info_Player.health == 0)
        {
            EventSystem.current.Death();
        }
    }

    IEnumerator Immune()
    {
        Info_Player.iframe = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(immuneTime);
        Info_Player.iframe = false;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
