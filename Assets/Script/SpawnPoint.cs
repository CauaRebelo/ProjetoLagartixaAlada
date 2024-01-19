using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent<bool> OnLit { get; set; }

    [SerializeField] private Transform spawnTransform;

    private bool used = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(!used)
            {
                OnLit?.Invoke(true);
                col.gameObject.transform.parent.gameObject.GetComponent<PlayerDamage>().spawnPoint = spawnTransform;
                used = true;
            }
        }
    }
}
