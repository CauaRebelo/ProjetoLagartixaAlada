using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateResourceBar(float value, float maxValue)
    {
        slider.value = value / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
