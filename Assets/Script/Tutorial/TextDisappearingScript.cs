using UnityEngine;
using UnityEngine.UI;

public class TextDisappearingScript : MonoBehaviour
{
    public Text tutorialText;
    private int tutorialStep = 0;


    void Start()
    {
        tutorialText.text = "Aperte A ou D para se movimentar";
    }

    void Update()
    {
        if (((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && tutorialStep == 0)
        {
            tutorialStep++;
            tutorialText.text = "";
        }
        else if(Input.GetButtonDown("Jump") && tutorialStep == 2){
            tutorialText.text = "";
        }
        else if(Input.GetButtonDown("Fire2") && tutorialStep == 3){
            tutorialText.text = "";
        }
        else if(Input.GetButtonDown("Fire1") && tutorialStep == 4){
            tutorialText.text = "";
              
        }else if(Input.GetButtonDown("Fire3") && tutorialStep == 5){
            tutorialText.text = "";
        }
        else if(Input.GetButton("Horizontal") && Input.GetButtonDown("Fire1") && tutorialStep == 6){
            tutorialText.text = "";
        }
        else if(Input.GetButton("Vertical") && Input.GetButtonDown("Fire1") && tutorialStep == 7){
            tutorialText.text = "";
        }
        
    }

    public void Jump()
    {
        tutorialText.text = "Aperte Espaço para pular";
        tutorialStep++;
    }

    public void Dash()
    {
        tutorialText.text = "Aperte Shift para usar o dash";
        tutorialStep++;
        
    }

    public void Attack()
    {
        tutorialText.text = "Aperte Z ou J para realizar um ataque";
        tutorialStep++;
        
    }

    public void Encantamento()
    {
        tutorialText.text = "Aperte L ou X para trocar de encantamento";
        tutorialStep++;
    }

    public void AttackVertical()
    {
        tutorialText.text = "Aperte W/Seta pra Cima e Z/J para executar um ataque vertical";
        tutorialStep++;
    }

    public void AttackHorizontal()
    {
        tutorialText.text = "Aperte alguma tecla de movimento horizontal e e Z/J para executar um ataque horizontal";
        tutorialStep++;
    }


}