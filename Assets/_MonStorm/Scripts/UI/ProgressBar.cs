using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private String barName;
    [SerializeField] private TextMeshProUGUI barLabel; 

    [Header("Values")]
    [SerializeField] public float currentValue;
    [SerializeField] public float maxValue;

    [Header("Bar Objects")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI progressLabel;
    void Awake()
    {
        barLabel.text = barName;
    }
    void Start()
    {
        progressLabel.text = currentValue + " / " + maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        if(maxValue != 0)
        {
            fillImage.fillAmount = currentValue / maxValue;
        }
        
    }

    public void SetValue(float value)
    {
        currentValue = value;
        progressLabel.text = currentValue + " / " + maxValue;
    }

    public void SetMax(float value)
    {
        maxValue = value;
        progressLabel.text = currentValue + " / " + maxValue;
    }


}
