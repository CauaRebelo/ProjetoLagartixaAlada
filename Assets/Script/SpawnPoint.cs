using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] private Transform spawnTransform;

    private bool used = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(!used)
            {
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>().spawnPoint = spawnTransform;
                used = true;
            }
        }
    }
}
