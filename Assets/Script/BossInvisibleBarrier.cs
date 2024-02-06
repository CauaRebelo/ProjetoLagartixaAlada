using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossInvisibleBarrier : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject dialogueFirst;
    [SerializeField] private GameObject dialogueAfter;
    [SerializeField] private GameObject afterBoss;
    [SerializeField] private GameObject newCamera;
    [SerializeField] private GameObject oldCamera;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject princess;
    private bool used = false;
    private bool bossDead = false;
    private bool firstTime = false;

    void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        EventSystem.current.onPlayerFinishTalking += OnPlayerFinishTalking;
        EventSystem.current.onBossDeath += OnBossDeath;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(used)
        {
            return;
        }
        else if(col.gameObject.tag == "Player")
        {
            wall.SetActive(true);
            used = true;
            newCamera.SetActive(true);
            oldCamera.SetActive(false);
            if(!firstTime)
            {
                dialogueFirst.SetActive(true);
                firstTime = true;
            }
            else
            {
                boss.SetActive(true);
                princess.SetActive(false);
            }
        }
    }

    void OnDeath()
    {
        wall.SetActive(false);
        used = false;
        newCamera.SetActive(false);
        oldCamera.SetActive(true);
        dialogueFirst.SetActive(false);
        dialogueAfter.SetActive(false);
        dialogueAfter.GetComponent<InstaDialogoNpc>().firstTime = true;
        princess.SetActive(true);
    }

    void OnPlayerFinishTalking()
    {
        if(bossDead)
        {
            SceneManager.LoadScene("Credits");
        }
        else
        {
            princess.SetActive(false);
            dialogueFirst.SetActive(false);
            dialogueAfter.SetActive(false);
            boss.SetActive(true);
        }
    }
    
    void OnBossDeath()
    {
        princess.SetActive(false);
        dialogueFirst.SetActive(false);
        dialogueAfter.SetActive(true);
        boss.SetActive(false);
        bossDead = true;
    }
}
