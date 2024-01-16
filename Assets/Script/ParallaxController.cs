using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    // Componentes
    [SerializeField] private GameObject mainCamera;

    // Atributos
    private float startPosition;
    private float length;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void LateUpdate()
    {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffect);
        float distance = mainCamera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (temp > startPosition + length)
        {
            startPosition += length;
        }
        else if (temp < startPosition - length)
        {
            startPosition -= length;
        }
    }

}