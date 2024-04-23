using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoNpc : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public Image imageNpc;
    public Sprite spriteNpc;

    public bool readyToSpeak;
    public bool startDialogue;

    void Start(){
        dialoguePanel.SetActive(false);
    }

    void Update(){
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && readyToSpeak){
            if(!startDialogue){
                FindObjectOfType<PlayerMovement>().isDialogueActive = true;
                
                StartDialogue();
            }
            else if(dialogueText.text == dialogueNpc[dialogueIndex]){
                NextDialogue();
            }
        }
    }

    void NextDialogue(){
        dialogueIndex++;

        if(dialogueIndex < dialogueNpc.Length){
            StartCoroutine(ShowDialogue());
        }
        else {
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            FindObjectOfType<PlayerMovement>().isDialogueActive = false;
            
        }
    }

    void StartDialogue(){
        imageNpc.sprite = spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue(){
        dialogueText.text = "";
        foreach(char letter in dialogueNpc[dialogueIndex]){
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            readyToSpeak = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            readyToSpeak = false;
        }
    }
}
