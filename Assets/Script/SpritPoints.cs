using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritPoints : MonoBehaviour
{
    public float spritPoints;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>().mudarSouls(spritPoints);
                spritPoints = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
