using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class timeSlider : MonoBehaviour
{
    public Slider timeslider;
    public Text text;
    public int seconds = 10;
    readonly Stopwatch timer = new Stopwatch();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CounDowntStarter()
    {
        TimeSpan timeStart = TimeSpan.FromSeconds(60f);
        while (true)
        {
            timer.Restart();
            while (timer.Elapsed.TotalSeconds <= seconds)
            {
                yield return new WaitForSeconds(0.01f);
                timeStart = TimeSpan.FromSeconds(seconds - Math.Floor(timer.Elapsed.TotalSeconds));
                text.text = string.Format("{0:00} : {1:00}", timeStart.Minutes, timeStart.Seconds);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
