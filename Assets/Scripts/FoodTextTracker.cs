using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FoodTextTracker : MonoBehaviour
{
   [SerializeField] private TMP_Text text; // Text mesh pro component.
    

    
    private string nextString="";

    // [SerializeField] private GameObject panel;
    
    private static float fontSize = 20.0f;
    private void Awake()
    {   


        StartCoroutine(Animate());
    }


   
    public void setOutString(string newString) {
        nextString = newString;
    }
    private IEnumerator Animate()
    {
        
        // panel.transform.sizeDelta = new Vector2((float)height*(fontSize+5), panel.transform.sizeDelta.y);
        while (true)
        {
            
            this.text.SetText(nextString);
            // RectTransform textRect = this.text.GetComponent<RectTransform>();
            // var height = nextString.Split('\n').Length;
            // RectTransform rect=  panel.GetComponent<RectTransform>();
            // LayoutRebuilder.MarkLayoutForRebuild(textRect);
            // LayoutRebuilder.MarkLayoutForRebuild(rect);
            yield return new WaitForSeconds(0.05f);
        }
    }

    
    

}
