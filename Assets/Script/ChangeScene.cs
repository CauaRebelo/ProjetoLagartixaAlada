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
}
