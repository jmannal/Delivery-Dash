using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoostBoxSpawnLogic : MonoBehaviour
{
    [SerializeField] GameObject itemBox;
    [SerializeField] int boxSpawnPercentage = 25;
    [SerializeField] public int boxCreated = 0;

    [SerializeField] private ParticleSystem collisionParticles;

    void Start()
    {
        // Choice of having some boost boxes already present at the start.
        if (boxCreated == 1)
        {
            var boostBox = Instantiate(this.itemBox);
            boostBox.transform.localPosition = new Vector3(this.transform.position.x, 70f, this.transform.position.z);

            ItemBoxBehaviour boxComponent = boostBox.GetComponent<ItemBoxBehaviour>();
            boxComponent.creator = this.gameObject;

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // Spawning boosts
        if (collision.tag == "Car")
        {

            Debug.Log("YWDGHGUWJFJWGFBKGWJFGHWBJF");


            if (boxCreated != 1)
            {
                System.Random rnd = new System.Random();
                int outcome = rnd.Next(1, 101);
                Debug.Log(outcome);
                if (outcome < boxSpawnPercentage)
                {
                    var particles = Instantiate(collisionParticles, transform.position, transform.rotation);

                    var boostBox = Instantiate(this.itemBox);
                    boostBox.transform.localPosition = new Vector3(this.transform.position.x, 70.5f, this.transform.position.z);
                    boxCreated = 1;
                    ItemBoxBehaviour boxComponent = boostBox.GetComponent<ItemBoxBehaviour>();
                    boxComponent.creator = this.gameObject;
                }
            }

        }
    }
}
