using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCollisionArea : MonoBehaviour
{


    [SerializeField] private Vector3 CollisionBoxRelative;
    public GameObject collisionBox;
  
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCollision() {
        Instantiate( collisionBox, this.transform.position + CollisionBoxRelative, new Quaternion(0,0,0,0), this.transform);
    }

    
}
