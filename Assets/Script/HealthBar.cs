using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateResourceBar(float value, float maxValue)
    {
        slider.value = value / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
