using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
   
    [SerializeField] private string prefix = "$";
    [SerializeField] private int defaultValue;
    [SerializeField] private float lerpSpeed;

    private float money;
    public string carryingCapacity;

    // Animation code based on UI demonstration in Workshop 6 https://github.com/COMP30019/Workshop-6-Solution
    private void Awake()
    {
        StartCoroutine(Animate());
    }

    public void SetValue(float value)
    {
        this.money = value;
    }

    private IEnumerator Animate()
    {
        float current = this.money = this.defaultValue;
        while (true)
        {
            current = Mathf.Lerp(current, this.money, this.lerpSpeed);
            
            UpdateText(current);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void UpdateText(float displayValue)
    {
        if (displayValue < 20) {
            this.text.color  = new Color(1f,0.1f,0f);
        } else if (displayValue < 50) {
            this.text.color  = new Color(1.0f, 0.64f, 0.0f);
        } else {
            this.text.color  = new Color(0.1f,1.0f,0.2f);
        }
        
        this.text.SetText(this.prefix + displayValue.ToString("F2") + '\n'+ carryingCapacity);
    }
  
}

