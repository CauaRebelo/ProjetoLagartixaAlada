using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeController : MonoBehaviour
{

    public static TreeController Instance;

    public bool[,] abilityTree = {{ false, false, false, false, false, false}, {false, false, false, false, false, false}, {false, false, false, false, false, false}, };

    public int pontosArvore;

    void Awake () 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad (transform.gameObject);
        Instance = this;
        pontosArvore = 5;
    }
    
    public bool AcessarArvore(int indexA, int indexB)
    {
        return abilityTree[indexA,indexB];
    }
    public void ModificarArvore(int indexA, int indexB, bool value)
    {
        abilityTree[indexA,indexB] = value;
    }

    public int AcessarPontos()
    {
        return pontosArvore;
    }
    public void ModificarPontos(int value)
    {
        pontosArvore += value;
    }
}
