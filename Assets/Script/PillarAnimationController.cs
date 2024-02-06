using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
