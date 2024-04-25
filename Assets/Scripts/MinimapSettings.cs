using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinimapSettings : MonoBehaviour
{


    [SerializeField] GameObject miniMap;
    public Transform targetToFollow;
    [SerializeField] Shader shader;
    [SerializeField]  Material minimapRender;
    [SerializeField] RenderTexture cameraIn;
    [SerializeField] RenderTexture cameraOut;
    public bool rotateWithTheTarget = true;
    private bool collidedNow = false;
    private bool boostedNow = false;


    private bool successNow = false;

    

    public void setFollow(Transform transform) {
        
        targetToFollow = transform;
    }
    // Start is called before the first frame update
    void Awake()
    {
    
        Debug.Log("awkened me");
        minimapRender= new Material( miniMap.GetComponent<RawImage>().material );
        minimapRender.SetFloat("_Saturation", 2.5f);
        minimapRender.SetFloat("_Frequency", 1f);
        minimapRender.SetFloat("_Brightness", 0.0f);
        minimapRender.SetFloat("_HeightSaturation", 0.7f);
        miniMap.GetComponent<RawImage>().material = minimapRender;
        

    }
    
 

    public void collided() {
        if (collidedNow==false) {
            collidedNow = true;
            minimapRender.SetFloat("_Saturation", 0.0f);
            StartCoroutine(collsionRun());
        }
        

        


    }


    IEnumerator collsionRun()
    {
        float start_time = Time.time;
        while (Time.time - start_time<1) {
            minimapRender.SetFloat("_Saturation",(Time.time - start_time)*2f);
            yield return new WaitForSeconds(.01f);
        }
        collidedNow=false;

        minimapRender.SetFloat("_Saturation", 2.5f);


    
        yield return null;
    }


    public void boost() {
        Debug.Log(boostedNow);
        if (boostedNow==false) {
            boostedNow = true;
            minimapRender.SetFloat("_Frequency", 10.0f);
            StartCoroutine(boostRun());
        }


    }


    public void setFuel(float height) {
       
        minimapRender.SetFloat("_HeightSaturation", height);
    }


    


    


    IEnumerator boostRun()
    {
        float start_time = Time.time;
        while (Time.time - start_time<3) {
            minimapRender.SetFloat("_Frequency",(10.0f- ((Time.time - start_time)*3.0f)));
            Debug.Log(10.0f- ((Time.time - start_time)*3.0f));
            yield return new WaitForSeconds(.01f);
        }
        boostedNow=false;
        Debug.Log(boostedNow);
        minimapRender.SetFloat("_Frequency", 1f);


    
        yield return null;
    }



    public void success() {
        if (successNow==false) {
            successNow = true;
            minimapRender.SetFloat("_Brightness", 1.0f);
            StartCoroutine(successRun());
        }
        

        


    }
    IEnumerator successRun()
    {
        Debug.Log(successNow);
        float start_time = Time.time;
        while (Time.time - start_time<1) {
            minimapRender.SetFloat("_Brightness",1.0f-(Time.time - start_time));
            yield return new WaitForSeconds(.01f);
        }
        successNow=false;

        minimapRender.SetFloat("_Brightness", 0.0f);


    
        yield return null;
    }

    // Update is called once per frame
    void Update() {
       
        miniMap.GetComponent<RawImage>().material= minimapRender;
        minimapRender= new Material( miniMap.GetComponent<RawImage>().material );
     
        
        
        
    }
}
