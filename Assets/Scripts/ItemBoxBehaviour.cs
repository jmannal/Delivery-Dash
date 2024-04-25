using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemBoxBehaviour : MonoBehaviour
{

    [SerializeField] public GameObject creator;

    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        // Get material
        this._material = GetComponent<MeshRenderer>().material;
        Debug.Log(creator);
        Debug.Log("lalalala");

    }

    // Update is called once per frame
    void Update()
    {
        // Change colour (change)
        var t = Time.time % 1f;
        var color = Color.HSVToRGB(t, 1f, 1f);

        this._material.color = color;

    }



}
