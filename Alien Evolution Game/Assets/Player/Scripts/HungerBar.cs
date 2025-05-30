using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public float food;
    public float maxFood;
    public int segments;
    public Slider slider;

    void Start()
    {
    }
    private void Update()
    {
        float sliderValue = food / maxFood;
        sliderValue = Mathf.Round(sliderValue * segments) / segments;
        slider.value = sliderValue;
    }
}
