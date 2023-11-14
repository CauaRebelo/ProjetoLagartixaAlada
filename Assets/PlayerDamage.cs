using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{  

    void Damage()
    {
        if(!Info_Player.iframe)
        {
            Info_Player.health--;
            EventSystem.current.PlayerDamage();
        }
    }
}
