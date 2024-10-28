using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimationScript : MonoBehaviour
{
    [SerializeField] private ElementalWallBehaviour wallBehaviour;

    public void Damaged()
    {
        wallBehaviour.RemovedDamaged();
    }

    public void Death()
    {
        wallBehaviour.RemovedDeath();
    }
}
