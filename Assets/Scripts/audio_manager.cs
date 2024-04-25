using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class audio_manager : MonoBehaviour
{

    [SerializeField] AudioClip drive;
    [SerializeField] AudioClip reverse;
    private AudioSource driveSource;
    private AudioSource dmgSource;
    private AudioSource reverseSource;

    private AudioSource boostSource;
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        Transform audioSource = transform.Find("AudioOperator");
        driveSource = audioSource.Find("Drive").GetComponent<AudioSource>();
        dmgSource = audioSource.Find("Damage").GetComponent<AudioSource>();
        reverseSource = audioSource.Find("Reverse").GetComponent<AudioSource>();
        boostSource = audioSource.Find("Boost").GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        double speed = Math.Pow(rigidBody.velocity.z, 2) + Math.Pow(rigidBody.velocity.x, 2);
 

        driveSource.volume = (float) (Math.Max(Math.Min(speed/1.5, 100), 20)/100);

        if (Input.GetKey(KeyCode.W) && !driveSource.isPlaying)
        {
            driveSource.clip= drive;
            Debug.Log(driveSource.clip);
            driveSource.Play();
        }
        else if (Input.GetKey(KeyCode.S) && !reverseSource.isPlaying)
        {
            reverseSource.clip= reverse;
            Debug.Log(reverseSource.clip);
            reverseSource.Play();
        
        } else if  (Input.GetKeyUp(KeyCode.S) && reverseSource.clip== reverse) {
            reverseSource.Stop();
        }
    }

    public void damageSound() {
        dmgSource.Play();
    }

    public void boostPlay(AudioClip clip){
        boostSource.clip = clip;
        boostSource.Play();
    }
}
