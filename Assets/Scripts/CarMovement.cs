using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarMovement : MonoBehaviour
{

    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;

    [SerializeField] ParticleSystem collisionParticles;

    [SerializeField] private float backwardAcceleration = -700f;
    [SerializeField] private float forwardAcceleration = 1000f;
    [SerializeField] private float drag = 200f;
    [SerializeField] private float brake = 2000f;

    private float currentAcceleration = 0f;
    private float currentBrake = 0f;

    private double maxCarSpeed = 400f;
    public double boostSpeed = 0f;
    private double speed;
    private Rigidbody rigidBody;

    public bool stopBoosting = false;
    [SerializeField] private float turnAmount = 40f;
    private float currentTurnAmount;
    private Camera camera;

    [SerializeField] private GameObject miniMap;
    private audio_manager audioSrc;

    private Vector3 lastVelocity;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSrc = GetComponent<audio_manager>();

        miniMap.GetComponent<MinimapSettings>().setFollow(transform);

        //camera = GetComponent<Camera>();
    }
    public void stopBoost()
    {
        if (!Input.GetKey(KeyCode.W))
        {
            frontLeft.brakeTorque = 5000000;
            frontRight.brakeTorque = 5000000;
            backLeft.brakeTorque = 5000000;
            backRight.brakeTorque = 5000000;
        }


    }
    void FixedUpdate()
    {
        speed = Math.Pow(rigidBody.velocity.z, 2) + Math.Pow(rigidBody.velocity.x, 2);

        float dif = (rigidBody.velocity - lastVelocity).magnitude / Time.fixedDeltaTime;
        lastVelocity = rigidBody.velocity;
        if (dif > 60)
        {
            this.gameObject.GetComponent<OrderCreator>().dealDamage(dif / 10);
            audioSrc.damageSound();
            Debug.Log("Damage Sound");
        }

        // Forward force when 'W' is pressed
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            currentAcceleration = forwardAcceleration - backwardAcceleration;
        }
        // Forwards acceleration when 'W' is pressed
        else if (Input.GetKey(KeyCode.W))
        {
            currentAcceleration = forwardAcceleration;
        }
        // Backwards acceleration when 'S' is pressed
        else if (Input.GetKey(KeyCode.S))
        {
            currentAcceleration = backwardAcceleration;
        }
        else
        {
            currentAcceleration = 0f;
            //currentBrake = drag;
        }

        // Hardbrake when Left Shift is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentBrake = brake;

        }
        else
        {
            currentBrake = 0f;
        }

        // Left turn when 'A' is pressed
        if (Input.GetKey(KeyCode.A))
        {
            currentTurnAmount = -turnAmount;
        }
        // Right turn when 'D' is pressed
        else if (Input.GetKey(KeyCode.D))
        {
            currentTurnAmount = turnAmount;
        }
        else
        {
            currentTurnAmount = 0f;
        }

        frontLeft.steerAngle = currentTurnAmount;
        frontRight.steerAngle = currentTurnAmount;




        double maxSpeed = maxCarSpeed;
        currentAcceleration += (float)boostSpeed;


        if ((currentBrake < 1) && (boostSpeed > 1 || maxSpeed > speed))
        {
            frontLeft.motorTorque = currentAcceleration;

            frontRight.motorTorque = currentAcceleration;
            backLeft.motorTorque = currentAcceleration;
            backRight.motorTorque = currentAcceleration;
        }
        else
        {
            frontLeft.motorTorque = 0f;
            frontRight.motorTorque = 0f;
            backLeft.motorTorque = 0f;
            backRight.motorTorque = 0f;
        }
        if (stopBoosting)
        {
            stopBoost();
        }
        else
        {
            frontLeft.brakeTorque = currentBrake;
            frontRight.brakeTorque = currentBrake;
            backLeft.brakeTorque = currentBrake;
            backRight.brakeTorque = currentBrake;
        }

        // transform.localEulerAngles = new Vector3(
        //   Mathf.Clamp(transform.rotation.eulerAngles.x, -45, 45),
        //   transform.rotation.eulerAngles.y,
        //   Mathf.Clamp(transform.rotation.eulerAngles.z, -45, 45)
        // );

        //camera.transform.rotation=Quaternion.Euler(new Vector3(90, 0, 0));
    }

    // void Update() {
    //     if (transform.eulerAngle.x > 45)
    // {
    //     transform.eulerAngle.x = 45;  
    // }
    // else if (transform.eulerAngle.x < 0)
    // {
    //     transform.eulerAngle.x = 0;
    //  }
    // }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "ItemBox")
        {

            var particles = Instantiate(this.collisionParticles, this.transform.position, this.transform.rotation);
            //particles.transform.position = transform.position;
            //particles.transform.rotation =
            //    Quaternion.LookRotation(-this.velocity);            
        }
    }



}