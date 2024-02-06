using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>() != null)
            {
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>().iframe = true;
            }
        }
    }
}
