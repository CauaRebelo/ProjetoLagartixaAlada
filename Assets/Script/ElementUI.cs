
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour
{
    [SerializeField] public PlayerMovement pM;

    public Image element;
    public Sprite ice;
    public Sprite fire;
    public Sprite thunder;
    public Sprite noElement;

    void Update(){

        if(pM.enchantment == 0){
        element.sprite = noElement;
        }
        if(pM.enchantment == 1){
            element.sprite = ice;
        }
        if(pM.enchantment == 2){
            element.sprite = fire;
        }
        if(pM.enchantment == 3){
            element.sprite = thunder;
        }

    }
}
