using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstaDialogoNpc : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public Text nameNpc;
    public Image imageNpc;
    public Sprite spriteNpc;

    public bool readyToSpeak;
    public bool startDialogue;
    public bool firstTime = true;

    void Start(){
        dialoguePanel.SetActive(false);
    }

    void Update(){
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && startDialogue){
            if(dialogueText.text != dialogueNpc[dialogueIndex]){
                StopAllCoroutines();
                dialogueText.text = dialogueNpc[dialogueIndex];
            }
            else {
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
        FindObjectOfType<PlayerMovement>().speed = 8f;
        EventSystem.current.PlayerFinishTalking();
    }
}

    void StartDialogue(){
        nameNpc.text = "Dragão Ancião";
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
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player") && !startDialogue && (firstTime || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))){
            firstTime = false;
            FindObjectOfType<PlayerMovement>().isDialogueActive = true;
            FindObjectOfType<PlayerMovement>().speed = 0f;
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
    if(collision.CompareTag("Player")){
        readyToSpeak = false;
    }
}
}