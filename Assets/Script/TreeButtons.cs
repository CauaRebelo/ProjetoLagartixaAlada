using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeButtons : MonoBehaviour
{
    public int aux1;
    public int aux2;
    private int unlock;
    public Button button;
    public PlayerMovement playerMovement;
    void Awake()
    {
        unlock = aux1 * 10 + aux2;
        if(playerMovement.checkTree(aux1, aux2))
        {
            button.interactable = false;
        }
        else if(aux2 % 2 == 1 && playerMovement.checkTree(aux1, aux2-1))
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
        EventSystem.current.onSkillChange += OnSkillChange;
    }

    public void TaskOnClick()
    {
        Debug.Log("Oi");
        EventSystem.current.SkillChange();
    }

    public void OnSkillChange()
    {
        unlock = aux1 * 10 + aux2;
        if(playerMovement.checkTree(aux1, aux2))
        {
            button.interactable = false;
        }
        else if(aux2 % 2 == 1 && !playerMovement.checkTree(aux1, aux2-1))
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
