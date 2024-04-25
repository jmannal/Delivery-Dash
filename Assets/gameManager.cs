using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player_parent;

    [SerializeField] GameObject light;
    void Start()
    {
        //Time.timeScale = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void setCar() {
        Debug.Log("setting car");
        Transform car = player_parent.transform.Find("Car");
        car.gameObject.SetActive(true);
        light.SetActive(true);
        player_parent.transform.Find("Main Camera").transform.SetParent(car);
        Destroy(gameObject);
    }
    public void setBus() {
        Debug.Log("setting bus");
        Transform bus = player_parent.transform.Find("Bus");
        bus.gameObject.SetActive(true);
        light.SetActive(true);
        player_parent.transform.Find("Main Camera").transform.SetParent(bus);
        Destroy(gameObject);
    }


    public void setStationWagon() {
        Debug.Log("setting wagon");
        Transform sta = player_parent.transform.Find("StationWagon");
        sta.gameObject.SetActive(true);
        light.SetActive(true);
        player_parent.transform.Find("Main Camera").transform.SetParent(sta);
        Destroy(gameObject);
    }
}
