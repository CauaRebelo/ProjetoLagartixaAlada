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
    [SerializeField] private GameObject normalSong;
    [SerializeField] private GameObject bossSong;
    [SerializeField] private GameObject victorySong;
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
                normalSong.SetActive(false);
                victorySong.SetActive(false);
                bossSong.GetComponent<MusicPlayer>().Play();
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
        bossSong.SetActive(false);
        victorySong.SetActive(false);
        normalSong.SetActive(true);
        normalSong.GetComponent<MusicPlayer>().Play();
        dialogueAfter.GetComponent<InstaDialogoNpc>().firstTime = true;
        princess.SetActive(true);
    }

    void OnPlayerFinishTalking()
    {
        if(bossDead)
        {
            SceneManager.LoadScene("Creditos");
        }
        else
        {
            bossSong.SetActive(true);
            normalSong.SetActive(false);
            victorySong.SetActive(false);
            bossSong.GetComponent<MusicPlayer>().Play();
            princess.SetActive(false);
            dialogueFirst.SetActive(false);
            dialogueAfter.SetActive(false);
            boss.SetActive(true);
        }
    }
    
    void OnBossDeath()
    {
        bossSong.SetActive(false);
        normalSong.SetActive(false);
        victorySong.SetActive(true);
        victorySong.GetComponent<MusicPlayer>().Play();
        princess.SetActive(false);
        dialogueFirst.SetActive(false);
        dialogueAfter.SetActive(true);
        boss.SetActive(false);
        bossDead = true;
    }
}
