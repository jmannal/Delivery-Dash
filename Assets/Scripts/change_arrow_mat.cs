using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_arrow_mat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // foreach(Transform child in transform) {
        //     var childRenderer = child.gameObject.GetComponent<Renderer>();
        //     childRenderer.material.SetColor("_Color", Color.red);
        // }
        
    }

    public void changeColour(Color color) {
        foreach(Transform child in transform) {
        var childRenderer = child.gameObject.GetComponent<Renderer>();
        childRenderer.material.SetColor("_Color",color);
        }
    }

    public void pointTo(Vector3 targetPoint) {
            Vector3 direction = new Vector3(targetPoint.x - transform.position.x, 0, targetPoint.z - transform.position.z);

            // Use Quaternion.LookRotation to determine the rotation to face the target
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the object
            transform.rotation = rotation;
    }

}
