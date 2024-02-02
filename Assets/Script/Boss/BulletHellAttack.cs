using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "BulletHell")
        {
            if(col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>() != null)
            {
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>().Damage();
            }
        }
    }
}
