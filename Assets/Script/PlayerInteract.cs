using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool interactableTouch;
    private bool dialogue;
    private bool spawnPoint;
    private bool sitting = false;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PlayerDamage playerDamage;
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && interactableTouch == true)
        {
            playerDamage.iframe = true;
            playerDamage.health = playerDamage.maxHealth;
            playerMovement.Sitdown();
            sitting = true;
            EventSystem.current.Death();
            StartCoroutine(Sit());
            canvas.gameObject.SetActive(true);
            if(spawnPoint)
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D teste)
    {
        if(teste.CompareTag("Interactable"))
        {
            interactableTouch = true;
        }
        if(teste.gameObject.name == "SpawnPoint")
        {
            spawnPoint = true;
            dialogue = false;
        }
        else
        {
            spawnPoint = false;
            dialogue = true;
        }
    }

    public void OnTriggerExit2D(Collider2D teste)
    {
        interactableTouch = false;
    }

    private IEnumerator Sit()
    {
        while(sitting == true)
        {
            playerDamage.iframe = true;
            playerDamage.health = playerDamage.maxHealth;
            playerMovement.Sitdown();
            yield return new WaitForSeconds(1f);
        }
    }

    public void Leave()
    {
        sitting = false;
        playerMovement.isAbleToAct = true;
        playerDamage.iframe = false;
    }
}
