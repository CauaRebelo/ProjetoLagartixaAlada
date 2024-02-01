using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeController : MonoBehaviour
{

    public static TreeController Instance;

    public bool[,] abilityTree = {{ false, false, false, false, false, false}, {false, false, false, false, false, false}, {false, false, false, false, false, false}, };

    public int pontosArvore;
    public float pontosEspirito;

    void Awake () 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad (transform.gameObject);
        Instance = this;
        pontosArvore = 1;
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

    public float AcessarPontosEspiritos()
    {
        return pontosEspirito;
    }
    public void ModificarPontosEspiritos(float value)
    {
        pontosEspirito += value;
    }
}
