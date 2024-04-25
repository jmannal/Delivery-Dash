using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TipDisplay : MonoBehaviour
{
   [SerializeField] private TMP_Text text; // Text mesh pro component.
    

    public bool finished = false;
    private Queue<string> nextString = new Queue<string>();
    

    private void Awake()
    {
        
        StartCoroutine(Animate());
    }


    public void setFinalText(string newString) {
        finished = true;
        this.text.SetText(newString);
    }
    public void setOutString(string newString) {
        nextString.Enqueue(newString);
    }
    private IEnumerator Animate()
    {
        
        
        while (true)
        {   
            if (finished) {
                Time.timeScale = 0;
                if (Input.GetKey(KeyCode.E)) {
                    Debug.Log("pressed");
                    Time.timeScale = 1;
                    SceneManager.LoadScene("Instructions");
                }
                yield return new WaitForSeconds(0.1f);
            }
            else if (nextString.Count != 0) {
                this.text.SetText(nextString.Dequeue());
                yield return new WaitForSeconds(2f);
            } else {
                this.text.SetText("");
                yield return new WaitForSeconds(0.05f);
            }
            
            

            
        }
    }

    
    

}
