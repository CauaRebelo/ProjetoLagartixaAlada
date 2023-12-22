using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void loadSceneFase1()
    {
        SceneManager.LoadScene("TesteInimigo");
    }

    public void loadSceneFase2()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void loadSceneFase3()
    {
        SceneManager.LoadScene("TesteInimigoSemAI");
    }

    public void loadSceneFase4()
    {
        SceneManager.LoadScene("TesteInimigosRawr");
    }
}
