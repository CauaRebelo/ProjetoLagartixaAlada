using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    public TextDisappearingScript scriptTutorial;

    public int etapa;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch (etapa)
            {
                case 0:
                    scriptTutorial.Dash();
                break;

                case 1: 
                    scriptTutorial.Jump();
                break;

                case 2: 
                    scriptTutorial.Attack();
                break;

                case 3:
                    scriptTutorial.Encantamento();
                break;

                case 4:
                    scriptTutorial.AttackHorizontal();
                break;

                case 5:
                    scriptTutorial.AttackVertical();
                break;
            }
            obj.SetActive(false);
        }
    }
    
}
