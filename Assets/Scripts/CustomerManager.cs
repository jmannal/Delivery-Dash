using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    [SerializeField] private float heatSatisfactionMod;
    
    [Range(0.0f,1.0f)]
    [SerializeField] private float damageSatisfactionMod;
    private LoadCollisionArea areaLoad;
    
    public void LoadArea()
    {
        
        areaLoad = this.GetComponent<LoadCollisionArea>();
        areaLoad.LoadCollision();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeatSat(float heatSatisfactionMod) {
        this.heatSatisfactionMod = heatSatisfactionMod;
    }
    public void SetDamageSat(float damageSatisfactionMod) {
        this.damageSatisfactionMod = damageSatisfactionMod;
    }

    public float GetHeatSat() {
        return heatSatisfactionMod;
    }
    public float GetDamageSat() {
        return damageSatisfactionMod;
    }

    public void DestroyMe() {
        Destroy(transform.GetChild(1).gameObject);
    }
    public string getName(){
        return this.name;
    }

    public void SetColor(Color color) {
        Transform custBox = this.transform.GetChild(1).transform;
        custBox.gameObject.GetComponent<Renderer>().material.SetColor("_Color",color);
        custBox.GetChild(0).transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color",color);
    }
}
