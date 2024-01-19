using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool interactableTouch;
    private bool dialogue;
    private bool spawnPoint;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PlayerDamage playerDamage;
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && interactableTouch == true)
        {
            playerMovement.isAbleToAct = false;
            playerDamage.iframe = true;
            playerDamage.health = playerDamage.maxHealth;
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

    public void Leave()
    {
        playerMovement.isAbleToAct = true;
        playerDamage.iframe = false;
    }
}
