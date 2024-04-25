using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    // Attach script to car
    //[SerializeField] GameObject car;
    [SerializeField] int boostMaxNumber = 4;
    [SerializeField] Sprite BoostButtonNull;
    [SerializeField] Sprite BoostButtonSpeed;
    [SerializeField] Sprite BoostButtonHeater;
    [SerializeField] Sprite BoostButtonTong;
    [SerializeField] Sprite BoostButtonFuel;
    [SerializeField] String vehicleName;
    [SerializeField] AudioClip pickUp;
    [SerializeField] AudioClip boostUse;
    [SerializeField] AudioClip fueluse;
    [SerializeField] AudioClip fireuse;
    [SerializeField] AudioClip tongsUse;

    [SerializeField] GameObject collisionParticles;
    private OrderCreator orderCreator;

    // create empty queue for boost (Linked list)
    LinkedList<int> boostQueue = new LinkedList<int>();
    CarMovement movementHolder;

    GameObject CanvasUI;
    private audio_manager audioSrc;
    private bool boostActive = false;



    void Start()
    {
        // Get necessary component (Food component info heat + damage)
        GameObject car = GameObject.Find(vehicleName);
        movementHolder = car.GetComponent<CarMovement>();
        orderCreator = car.GetComponent<OrderCreator>();
        CanvasUI = GameObject.Find("Canvas");
        audioSrc = GetComponent<audio_manager>();
        collisionParticles = GameObject.Find("ItemBoxExplosion");

        UpdateBoostUI();

    }

    // Update is called once per frame
    void Update()
    {
        // Activating boosts
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (boostQueue.Count > 0 && boostActive == false)
            {
                ActivateBoost();

                boostQueue.RemoveFirst();
                UpdateBoostUI();
            }
            else
            {
                Debug.Log("No boosts currently held");
            }
        }

    }

    private void UpdateBoostUI()
    {
        // Update Boost UI
        for (int i = 0; i < boostMaxNumber; i++)
        {
            GameObject thisBoost = CanvasUI.transform.GetChild(i).gameObject;
            Image boostImage = thisBoost.GetComponent<Image>();

            if (i < boostQueue.Count)
            {
                LinkedListNode<int> boostNumber = boostQueue.First;
                boostQueue.RemoveFirst();
                boostQueue.AddLast(boostNumber);

                Sprite boostSprite = ChooseSprite(boostNumber.Value);
                boostImage.sprite = boostSprite;

            }
            else
            {
                // set ith boost UI to the empty picture
                boostImage.sprite = BoostButtonNull;
            }
        }
    }

    private Sprite ChooseSprite(int SpriteNumber)
    {
        if (SpriteNumber == 1)
        {
            return BoostButtonSpeed;
        }
        else if (SpriteNumber == 2)
        {
            return BoostButtonHeater;
        }
        else if (SpriteNumber == 3)
        {
            return BoostButtonTong;
        }
        else if (SpriteNumber == 4)
        {
            return BoostButtonFuel;
        }

        return BoostButtonNull;
    }



    private void ActivateBoost()
    {
        int activeBoost = boostQueue.First.Value;

        if (activeBoost == 1)
        {
            SpeedBoost();
        }
        else if (activeBoost == 2)
        {
            HeaterBoost();
        }
        else if (activeBoost == 3)
        {
            TongBoost();
        }
        else if (activeBoost == 4)
        {
            FuelBoost();
        }
    }

    private void SpeedBoost()
    {
        audioSrc.boostPlay(boostUse);
        orderCreator.usedBoost();
        Debug.Log("Speed Boost activated");
        // Increase max speed by some variable (currently 25%)
        movementHolder.boostSpeed = 10000.0f;

        StartCoroutine(SlowCar());

    }


    private IEnumerator SlowCar()
    {
        float currentTime = 0;
        float total_time = 0;

        boostActive = true;

        float startFoV = Camera.main.fieldOfView;
        while (total_time < 3)
        {

            currentTime = Time.deltaTime;
            total_time += currentTime;

            if (Camera.main.fieldOfView < 130)
            {
                Camera.main.fieldOfView += 35 * currentTime;
            }


            if (currentTime < 1)
            {
                movementHolder.boostSpeed = movementHolder.boostSpeed + 15000000.0 * currentTime;
            }

            yield return null;
        }

        movementHolder.boostSpeed = 0;
        boostActive = false;
        while (Camera.main.fieldOfView > startFoV)
        {
            movementHolder.stopBoosting = true;
            currentTime = Time.deltaTime;

            Camera.main.fieldOfView -= 45 * currentTime;


            yield return null;
        }
        movementHolder.stopBoosting = false;



        yield break;

    }


    private void HeaterBoost()
    {
        audioSrc.boostPlay(fireuse);
        orderCreator.addHeat(15);
        // Add heat back to food (choose one with lowest heat)
        Debug.Log("Heater Boost activated");
    }

    private void TongBoost()
    {
        audioSrc.boostPlay(tongsUse);
        orderCreator.removeDamage(15);
        // Remove damage done to food (choose one with most damage)
        // Could randomise this (and heater boost) with a variable
        Debug.Log("Tong Boost activated");
    }

    private void GhostBoost()
    {
        Debug.Log("Ghost Boost activated");
        // change damage to car to 0
        // Change the car colour so that it's obvious
        // Just before boost expires, flash between normal car colour and ghost car colour
        // After time expires, remove boost
    }
    private void FuelBoost()
    {

        audioSrc.boostPlay(fueluse);
        orderCreator.addFuelBoost(25);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "ItemBox")
        {

            if (boostQueue.Count < boostMaxNumber)
            {
                audioSrc.boostPlay(pickUp);
                // Randomize boost
                System.Random rnd = new System.Random();
                int boost = rnd.Next(1, 5);
                // 1 - speed, 2 - heater, 3 - tongs, 4 - fuel

                Debug.Log(boost);
                boostQueue.AddLast(boost);
            }

            // Display on UI
            UpdateBoostUI();

            // Send message to ItemBoxSpawner that the box is empty and they can spawn a new one
            ItemBoxBehaviour getCreator = collision.gameObject.GetComponent<ItemBoxBehaviour>();
            BoostBoxSpawnLogic Creator = getCreator.creator.GetComponent<BoostBoxSpawnLogic>();
            Creator.boxCreated = 0;

            Debug.Log("HELLO FRIEND");

            // Destroy box + (play sound effect)
            Destroy(collision.gameObject);

        }



    }


}
