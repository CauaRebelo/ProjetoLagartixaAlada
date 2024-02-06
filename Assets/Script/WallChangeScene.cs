using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallChangeScene : MonoBehaviour
{

    public int index;

    public void loadSceneFase1()
    {
        SceneManager.LoadScene("L1P2");
    }

    public void loadSceneFase2()
    {
        SceneManager.LoadScene("L1P3");
    }

    public void loadSceneFase3()
    {
        SceneManager.LoadScene("L1P4");
    }

    public void loadSceneFase4()
    {
        SceneManager.LoadScene("L1P5");
    }

    public void loadSceneComeco()
    {
        SceneManager.LoadScene("L1P1");
    }

    public void loadSceneMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void loadSceneFase6()
    {
        SceneManager.LoadScene("L1P6");
    }

    public void loadSceneFase7()
    {
        SceneManager.LoadScene("L1P7");
    }

    public void loadSceneFase8()
    {
        SceneManager.LoadScene("L2P1");
    }

    public void loadSceneFase9()
    {
        SceneManager.LoadScene("L2P2");
    }
    public void loadSceneFase10()
    {
        SceneManager.LoadScene("L3Boss");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(index == 1)
            {
                loadSceneFase1();
            }
            if(index == 2)
            {
                loadSceneFase2();
            }
            if(index == 3)
            {
                loadSceneFase3();
            }
            if(index == 4)
            {
                loadSceneFase4();
            }
            if(index == 5)
            {
                loadSceneFase6();
            }
            if(index == 6)
            {
                loadSceneFase7(); 
            }
            if (index == 7)
            {
                loadSceneFase8();
            }
            if (index == 8)
            {
                loadSceneFase9();
            }
            if (index == 9)
            {
                loadSceneFase10();
            }
        }
    }
}
