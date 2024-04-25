using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float timeLeft = 300;




    private OrderCreator orderCreator;

    int secondsInMinute = 60;
    private bool startedTime = false;

    // Start is called before the first frame update
    void Start()
    {
        startedTime =true;
        orderCreator = this.GetComponent<OrderCreator>();
    }

    public void startTime(){
        startedTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedTime == false) {
            float startSeconds = timeLeft % secondsInMinute;
            int startMinutesLeft = (int) Math.Floor(timeLeft / (double) secondsInMinute);
            string StartTimeText = "Time Left: " + startMinutesLeft.ToString() + ":0" + startSeconds.ToString("F2");
            this.text.SetText(StartTimeText);
            return;
        }
        timeLeft -= Time.deltaTime;        


        float secondsLeft = timeLeft % secondsInMinute;
        // Check if the floor function is working
        int minutesLeft = (int) Math.Floor(timeLeft / (double) secondsInMinute);

        string timeText = "";
        
        if (secondsLeft < 10)
        {
            timeText = "Time Left: " + minutesLeft.ToString() + ":0" + secondsLeft.ToString("F2");    
        }
        else
        {
            timeText = "Time Left: " + minutesLeft.ToString() + ":" + secondsLeft.ToString("F2");
        }

        UpdateTimeText(timeText);

        if (timeLeft < 0.05)
        {
            string endTimeText = "Time Left: 0:00.00";
            UpdateTimeText(endTimeText);
            orderCreator.GameOver();
            // SceneManager.LoadScene(0);
        }


        /*
        // Restarts Scene
        if (Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
        
    }

    private void UpdateTimeText(string timeText)
    {
        this.text.SetText(timeText);
    }
}
