using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0,100)]
    [SerializeField] private float startingHeat;
    [Range(0,100)]
    [SerializeField] private float heatLoss;
    [Range(0,100)]
    [SerializeField] private float currentDamage;
    [Range(0,1)]
    [SerializeField] private float DamageTaker;
    [SerializeField] private Color color;


    [SerializeField] private string name;
    private float currentHeat;
    private bool pickedUp;
    void Start()
    {
        currentHeat = startingHeat;
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp == true) {
            currentHeat = currentHeat - (heatLoss* Time.deltaTime);
           
            if (currentHeat < 0) {
                currentHeat = 0;
                
            }
            
        


        }
        
        
    }
    public void setPickedUp(bool b, Transform NewParent) {
        pickedUp = b;
        //this.transform.parent = NewParent;
        Debug.Log("Picked up: " + this.name);
    }
    public float getHeat() {
        return currentHeat;
    }
    public void addHeat(float heat) {

        this.currentHeat += heat;
        if (currentHeat>100){
            currentHeat = 100;
        }
    }
    public void applyDamage(float damage) {
        currentDamage += DamageTaker*damage;
        if (currentDamage > 100) {
            currentDamage = 100;
        }
        if (currentDamage < 0) {
            currentDamage = 0;
        }
    }
    public float getDamage() {
        return currentDamage;
    }
    public string getName() {
        return this.name;
    }

    public Color GetColor() {
        return color;
    }

}
