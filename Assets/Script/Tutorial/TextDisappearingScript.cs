using UnityEngine;
using UnityEngine.UI;

public class TextDisappearingScript : MonoBehaviour
{
    public Text tutorialText;
    private int tutorialStep = 0;

    void Start()
    {
        tutorialText.text = "Aperte D ou Seta Direita para avançar";
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && tutorialStep == 0)
        {
            tutorialStep++;
            tutorialText.text = "Aperte A ou Seta Esquerda para voltar";
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && tutorialStep == 1)
        {
            tutorialStep++;
            tutorialText.text = "Aperte Espaço para pular";
        }
        else if ((Input.GetKeyDown(KeyCode.LeftShift)) && tutorialStep == 3)
        {
            tutorialStep++;
            tutorialText.text = "Espaço + SHIFT para usar um AirDash";
        }
        else if ((Input.GetKeyDown(KeyCode.LeftShift)) && tutorialStep == 4)
        {
            tutorialStep++;
            tutorialText.text = "aaaaaa ";
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            tutorialText.text = "Aperte Espaço para pular";
        }
    }

}