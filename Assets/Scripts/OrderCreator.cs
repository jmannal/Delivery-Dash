using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderCreator : MonoBehaviour
{
    
    [SerializeField] GameObject restaurant_parent;
    [SerializeField] GameObject orderUIobj;

    
    [SerializeField] GameObject TipUIobj;

    [SerializeField] GameObject miniMapParent;
    [SerializeField] GameObject customer_parent;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int MaxCapacity;
    [SerializeField] float StartingMoney = 35;

    [SerializeField] float StartingFuel = 75;
    [SerializeField] AudioClip delivered;
    [SerializeField] AudioClip pickedup;
    [SerializeField] AudioClip petrol;

    [SerializeField] GameObject fuelText;
    private Fueltext fueltextOut;

    [SerializeField] GameObject moneyText;
    private MoneyText moneytextOut;

    [SerializeField] AudioClip over;
    private audio_manager audioSrc;

    private int carryingCapacity;
    private List<GameObject> rest_list= new List<GameObject>();
    private List<GameObject> cust_list= new List<GameObject>();
    private List<GameObject> finished_cust =new List<GameObject>();
    private List<GameObject> finished_rest =new List<GameObject>();
    private List<Order> orders = new List<Order>();

    private Dictionary<Order, GameObject> orderArrows = new Dictionary<Order, GameObject>();

    private FoodTextTracker foodText;

    private MinimapSettings minimapRender;
    private string orderUI;

    private int orderTally =0;
 


     private TipDisplay tipText;

    private float fuel;

    private float Maxfuel=100;
    private int custCount;
    private float tipTotal =0;
    
    private float last_created_time = 0;
    private float last_check=0;
    private bool successPlayed = false;
    private MinimapSettings minimapSettings;
    void Start()
    {
        successPlayed = false;
        last_created_time = Time.time;
        last_check=0;
        tipTotal =0;
        orderTally =0;
        orders = new List<Order>();
        rest_list= new List<GameObject>();
        fueltextOut = fuelText.GetComponent<Fueltext>();
        moneytextOut = moneyText.GetComponent<MoneyText>();
        cust_list= new List<GameObject>();
        finished_rest =new List<GameObject>();
        finished_cust =new List<GameObject>();
        orderArrows = new Dictionary<Order, GameObject>();
        minimapSettings = miniMapParent.GetComponent<MinimapSettings>();
        fuel= StartingFuel;
        fueltextOut.SetValue(fuel);
       
        tipTotal = StartingMoney;
        moneytextOut.SetValue(tipTotal);
        audioSrc = GetComponent<audio_manager>();
        foodText= orderUIobj.GetComponent<FoodTextTracker>();
        tipText= TipUIobj.GetComponent<TipDisplay>();
        foreach (Transform child in restaurant_parent.transform) {
            rest_list.Add(child.gameObject);
            Debug.Log(child);
        }

        foreach (Transform child in customer_parent.transform) {
            cust_list.Add(child.gameObject);
            Debug.Log(child);
        }
        custCount = cust_list.Count;
        Debug.Log("Creating Order again");
        createOrder();
  
    }
    public void addFuel(float fuel){
        tipText.setOutString("Refueld "+ fuel.ToString("F1") + "L");
        this.fuel += fuel;
    }

    public void addFuelBoost(float fuel) {
        minimapSettings.boost();
        addFuel(fuel);
    }

    public void usedBoost() {
        minimapSettings.boost();
    }
    // Update is called once per frame
    void Update()
    {
        if (tipText.finished) {
            if (successPlayed == false && fuel > 0){
                audioSrc.boostPlay(over);
                successPlayed = true;
            }
            if (Input.GetKey(KeyCode.E)) {
            Debug.Log("pressed");
            Time.timeScale = 1;
            SceneManager.LoadScene("StartScene");
            } else {
                return;
            }
            
        }



        fuel -= Time.deltaTime;

        fueltextOut.SetValue(fuel);
        moneytextOut.SetValue(tipTotal);
        moneytextOut.carryingCapacity ="Capacity: " + carryingCapacity.ToString() + "/" + MaxCapacity.ToString()+"\n";
        minimapSettings.setFuel(fuel/Maxfuel);
        
        if (fuel < 0) {
            fuel=0.0f;
            tipText.setFinalText("Game Over. You ran out of fuel, in total you recieved $" + tipTotal.ToString("F2") + " worth of tips! Press E to restart!");
        }
        if (cust_list.Count > 0) {


            if (rest_list.Count <= 0) {
                rest_list = finished_rest;
                finished_rest =new List<GameObject>();
                
            }
            if (cust_list.Count < 2) {
                cust_list.AddRange(finished_cust);
                finished_cust = new List<GameObject>();

            }
            if (rest_list.Count > 0) {
                last_check += Time.deltaTime;
                if ((Time.time - last_created_time > 25 || (last_check > 1 && Random.Range(0,1000)==5))|| orders.Count == 0) {
                    createOrder();
                    last_created_time = Time.time;
                    last_check = 0;
                }
            }
            
        }

        if (orders.Count == 0) {
            orderUI = "No orders to pick up";
        } else {
            orderUI = "";
            foreach (Order order in orders) {
                Vector3 target = order.getTargetPosition();

                var arrow = orderArrows[order];
                arrow.GetComponent<change_arrow_mat>().pointTo(target);
                //Debug.Log(order.getName());
        
                orderUI += order.getText();
                
            }
          
        }
       
      
        foodText.setOutString(orderUI);
        
    }


    public void addHeat(float heat) {
        minimapSettings.boost();
        
         foreach (Order order in orders) {
            order.addHeat(heat);
        }

        if (carryingCapacity == 0) {
            addFuel(10);
        } else {
            tipText.setOutString("Restored heat to food!");
        }
    }
    public void removeDamage(float dmg) {
        minimapSettings.boost();
        foreach (Order order in orders) {
            order.dealDamage(-1*dmg);
        }
        if (carryingCapacity == 0) {
            addFuel(10);
        } else {
            tipText.setOutString("Removed damage from food!");
        }
    }

    public void dealDamage(float dmg) {
        minimapSettings.collided();
        foreach (Order order in orders) {
            order.dealDamage(dmg);
        }
    }

    void createOrder() {
        orderTally += 1;
        var customer = cust_list[Random.Range (0,(cust_list.Count))];
        cust_list.Remove(customer);
        Debug.Log("Rest_List length: " + rest_list.Count.ToString());
        var restaurant = rest_list[Random.Range (0,(rest_list.Count))];

        // print all restaurants out
        // foreach (GameObject Rest in rest_list) {
        //     Debug.Log(Rest.name);   
        // }


        rest_list.Remove(restaurant);
        
        Debug.Log("Created Order");
        Debug.Log(customer);
        Debug.Log(restaurant);

        Order order=new Order(customer, restaurant, this.transform.gameObject);
        orders.Add(order);
        GameObject arrow = Instantiate(arrowPrefab, this.transform.position + new Vector3(0.0f, (0.005f*orderTally)+0.5f,0.0f), new Quaternion(0,0,0,0), this.transform);
        arrow.GetComponent<change_arrow_mat>().changeColour(order.GetColor());
        orderArrows.Add(order, arrow);


    }

    void Refuel() {
        
        
        if (Input.GetKeyUp(KeyCode.Q) && fuel < 95.0f) {
            if (tipTotal >= 25.0) {
                audioSrc.boostPlay(petrol);
                tipTotal -= 25;
                fuel+= 30;
                
                
                minimapSettings.boost();
                if (fuel > Maxfuel) {
                    fuel = Maxfuel;
                }
                tipText.setOutString("Refueld up to "+ fuel.ToString("F1") + "L\n for $25");
                
            
            } else {
                Debug.Log("Not enought money to refuel");
            }

        }
    }

    void OnTriggerStay(Collider collision) {
        
        List<Order> orders_to_remove = new List<Order>();

        if (collision.gameObject.name == "FuelArea") {
            Refuel();
        }

        foreach (Order order in orders) {
            int x = order.Collided(collision, carryingCapacity, MaxCapacity);
            if (x==1) {
                minimapSettings.success();
                audioSrc.boostPlay(pickedup);
            }

            if (x == -1) {
                minimapSettings.success();
                finished_rest.Add(order.getRestaurant());
                finished_cust.Add(order.getCustomer());
                tipTotal += order.getTip();
                tipText.setOutString("You recieved a $" + order.getTip().ToString("F2") + " tip!");
                orders_to_remove.Add(order);
                audioSrc.boostPlay(delivered);
                
            }

            carryingCapacity += x;
        }
        foreach (Order order in orders_to_remove) {
            Debug.Log(order.getName());
            GameObject arrow = orderArrows[order];
            Destroy(arrow);
            orders.Remove(order);
        } 

        if (finished_cust.Count == custCount) {
            audioSrc.boostPlay(over);
            tipText.setFinalText("Game Over. In total you recieved $" + tipTotal.ToString("F2") + " worth of tips! Press E to restart!");
        }

    }


    public void GameOver() {
        tipText.setFinalText("Game Over. In total you recieved $" + tipTotal.ToString("F2") + " worth of tips!");
    }
}
