using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeController : MonoBehaviour
{

    public static TreeController Instance;

    public bool[,] abilityTree = {{ false, false, false, false, false, false}, {false, false, false, false, false, false}, {false, false, false, false, false, false}, };

    void Awake () 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad (transform.gameObject);
        Instance = this;
    }
    
}
