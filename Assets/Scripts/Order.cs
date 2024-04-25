using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{

    private GameObject customer;
    private GameObject restaurant;
    private GameObject car;
    
    private FoodMonitor pickedUpFood;
    private bool HoldingFood;
    private Restaurant rest;
    private CustomerManager cust;
    private float tip;
    
   

    public Order(GameObject customer, GameObject restaurant, GameObject car) {
        this.customer = customer;
        this.restaurant = restaurant;
        this.car = car;
        rest = restaurant.GetComponent<Restaurant>();
        cust = customer.GetComponent<CustomerManager>();
        rest.CreateFood();
       
    }
    public Color GetColor() {
        return rest.getFood().GetColor();
    }
    public GameObject getRestaurant() {
        return restaurant;
    }

    public GameObject getCustomer() {
        return customer;
    }

    public void addHeat(float heat){
        if (HoldingFood) {
            pickedUpFood.addHeat(heat);
        }
    }

    
    public void dealDamage(float dmg){
        if (HoldingFood) {
            pickedUpFood.applyDamage(dmg);
        }
    }


    public Vector3 getTargetPosition(){
        if (HoldingFood) {
            return customer.transform.position;

        } else {
            return restaurant.transform.position;
        }
    }
    public string getText() {
        if (!HoldingFood) {
            return "Go pick up food from: " + restaurant.GetComponent<Restaurant>().getName() + "\n";
        } else {
            
            return pickedUpFood.getName() + " dmg: "+ pickedUpFood.getDamage().ToString("F2") + "%\n"+
                "heat: " + pickedUpFood.getHeat().ToString("F2") + "%\n"
                + " Deliver to: " + cust.getName() +"\n";
        }
        return "Error";
    }

    public int Collided(Collider collision, int carryingCapacity, int MaxCapacity)
    {
        
        
        if (!HoldingFood && collision.gameObject == restaurant.transform.gameObject)
        {
            
            if (carryingCapacity + 1 > MaxCapacity) {
                Debug.Log("Can't pick up, exceeded capactiy");
                return 0;
            }
            pickedUpFood = rest.getFood();

            HoldingFood = true;
            pickedUpFood.setPickedUp(true, car.transform);
            //foodText.SetPickedUp(true);
            Debug.Log("Picked up");
            // Destroy the restaurant trigger area
            rest.DestroyMe();
            cust.LoadArea();
            cust.SetColor(pickedUpFood.GetColor());

            return 1;
                
            
        }

        if (HoldingFood && collision.gameObject == customer.transform.GetChild(1).transform.gameObject)
        {
           
            Debug.Log("collided with cust");
            // Give food + receive tip
            // Receive a new order if no other order currently
            // Destroy Customer House trigger area
            float custHeatSat = cust.GetHeatSat();
            float foodHeat = pickedUpFood.getHeat();
            float custDmgSat = cust.GetDamageSat();
            
            float foodDmg = pickedUpFood.getDamage();
            pickedUpFood.setPickedUp(false, car.transform);
            tip = Mathf.Floor(calcTip(10, foodHeat, custHeatSat, custDmgSat, foodDmg));
            Debug.Log((foodDmg/100) * custDmgSat);
            Debug.Log(tip);
            
            
            HoldingFood = false;
            cust.DestroyMe();
            return -1;

        }
        return 0;
        // if (HoldingFood) {
        //     Debug.Log(customer.transform.GetChild(0).transform.gameObject == collision.gameObject);
            
        // }
    }

    private float calcTip(float baseTip, float foodHeat, float custHeatSat, float foodDmg, float custDmgSat) {
        return Mathf.Max(baseTip * ((foodHeat/4.5f) * custHeatSat - (foodDmg/4.5f) * custDmgSat), 0);
    }

    public float getTip() {
        return tip;
    }
    public string getName(){
        return cust.getName();
    }
    public string getFood(){
        return pickedUpFood.getName();
    }

    
}
