using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restaurant : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] private GameObject foodPrefab;
    
    private FoodMonitor food;
    [SerializeField] private string name;

    private GameObject createdFood;
    
    void Start()
    {

        // this.transform.position = position;
        // foods_list=Resources.LoadAll<GameObject>("Food_Prefabs");
   
        // CreateFood();
	

    }
    void OnDestroy() {
        HideFood();
    }


    public void CreateFood() {
        //GameObject food_current = foods_list [Random.Range (0, foods_list.Length)];
        createdFood = Instantiate(foodPrefab, transform.position, Quaternion.identity, this.transform) as GameObject;
        food = createdFood.GetComponent<FoodMonitor>();

        // var childRenderer = child.gameObject.GetComponent<Renderer>();
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color",food.GetColor());
        this.transform.GetChild(0).transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color",food.GetColor());
        food.transform.localScale += new Vector3(4,10,4);
    }
    public FoodMonitor getFood() {
        return food;
    }

    void HideFood() {
        if (createdFood) {
             createdFood.GetComponent<Renderer>().enabled = false;
             gameObject.GetComponent<Renderer>().enabled = false;
        }
       
    }

    public void ShowReasturont() {
        this.CreateFood(); 
        createdFood.GetComponent<Renderer>().enabled = true;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyMe() {
        createdFood.GetComponent<Renderer>().enabled = false;
    }

    public string getName() {
        return this.name;
    }
}
