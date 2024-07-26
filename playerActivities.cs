using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.VFX;
using static System.Net.Mime.MediaTypeNames;

public class playerActivities : MonoBehaviour
{
    public bool boots = false, canFire = false, canMilk = false, fireDmg = false, haveMatchbox = false;
    public int food = 0, sticks = 0, inventorySize = 5, crafting = 0, buildFirePlace = 0;
    readonly Stopwatch timer = new Stopwatch();
    public float currentStamina = 7f, currentHealth = 7f, incrVignette = .2f, inventory = 0, seconds = 10f;
    public Transform lboot, rboot,kuboos, campTimer;
    public ParticleSystem fire;
    public Slider health,stamina;
    public UnityEngine.UI.Text foodCount;
    public GameObject campFire, emptyCampFire, campfireLight;
    public UnityEngine.UI.Image vignette, hitVignette, firePlaceTimer;

    private void Start()
    {   
        fire.Stop();
    }

    private void FixedUpdate()
    {
        inventoryCalculator();
        if (gameObject.GetComponent<Rigidbody>().IsSleeping())
        {
            gameObject.GetComponent<Rigidbody>().WakeUp();
        }
        if (Input.GetKeyDown("q") && food > 0)
        {
            food--;
            foodCount.text = food.ToString();
            if (currentStamina < 10f)
            {
                currentStamina++;
                stamina.value = currentStamina;
            }
        }
        if (Input.GetKeyDown("e"))
        {
            if (canMilk == true)
            {
                currentHealth++;
                health.value = currentHealth;
            }
            if (canFire == true)
            {
                campfireLight.SetActive(true);
                fire.Play();
                fireDmg = true;
            }
            if (crafting == 1 && sticks > 3)
            {
                campTimer.gameObject.SetActive(true);
                buildFirePlace ++;
                
            }
        }
        if(currentHealth < 5f)
        {
            incrVignette = 1f;
            StartCoroutine(AddVignette());

        }
        else if (currentHealth < 6f)
        {
            incrVignette = 0f;
            StartCoroutine(AddVignette());
        }
    }


    private void inventoryCalculator()
    {
        if(food != 0)
        {
            inventory = 1;
        }
        inventory = sticks+1;
    }
    private void Update()
    {
        if(buildFirePlace==1)
        {   
            seconds -= Time.deltaTime;
            firePlaceTimer.fillAmount = seconds/10;
            if (firePlaceTimer.fillAmount == 0f)
            {
                buildFirePlace++;
                emptyCampFire.SetActive(false);
                campFire.SetActive(true);
                sticks -= 4;
                crafting = 2;
                campTimer.gameObject.SetActive(false);
            }
        }

        campTimer.LookAt(gameObject.transform);

    }
    private IEnumerator AddVignette()
    {
        var alpha = vignette.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / .1f)
        {
            var newColor = new Color(0f, 0f, 0f, Mathf.Lerp(alpha, ((1f - currentHealth/10f)+.2f)*incrVignette, .5f));
            vignette.color = newColor;
            yield return null;
        }
    }
    private IEnumerator HitEffect()
    {
        var alpha = hitVignette.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / .4f)
        {
            hitVignette.color = new Color(255f, 0, 0, Mathf.Lerp(alpha, 1f , .4f));
 
            yield return null;
        }
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / .1f)
        {
            hitVignette.color = new Color(0f, 0f, 0f, Mathf.Lerp(alpha, 0f, .4f));
            yield return null;
        }
    }
    IEnumerator buildTimer(UnityEngine.UI.Image timerImg)
    {
        TimeSpan timeStart = TimeSpan.FromSeconds(60f);
        while (true)
        {
            timer.Restart();
            while (timer.Elapsed.TotalSeconds <= seconds)
            {
                yield return new WaitForSeconds(0.01f);
                timeStart = TimeSpan.FromSeconds(seconds - Math.Floor(timer.Elapsed.TotalSeconds));
                timerImg.fillAmount -= 0.1f;
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
    IEnumerator FireDamage()
    {
        fireDmg = false;
        yield return new WaitForSeconds(1f);
        currentHealth--;
        StartCoroutine(HitEffect());
        health.value = currentHealth;
        fireDmg = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "boots" && boots == false)
        {
            lboot.gameObject.SetActive(true);
            rboot.gameObject.SetActive(true);
            GameObject.Destroy(other.gameObject);
        }
        if (other.tag == "food")
        {   food+= 5;
            GameObject.Destroy(other.gameObject);
            foodCount.text = food.ToString();
        }
        if (other.tag == "matchbox")
        {
            haveMatchbox = true;
            GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "sheep" && currentHealth < 10f)
        {
            canMilk = true;
        }
        if (other.gameObject.tag == "fire" && haveMatchbox == true && crafting == 2)
        {
            canFire = true;
        }
        if (other.gameObject.tag == "tree" && inventory < inventorySize)
        {
            sticks += 1;
            GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "nofire")
        {
            crafting = 1;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "fire" && fireDmg == true)
        {
            StartCoroutine(FireDamage());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "sheep")
        {
            canMilk = false;
        }
        if (other.gameObject.tag == "fire")
        {
            canFire = false;
        }
        if (other.gameObject.tag == "nofire")
        {
            crafting = 0;
        }
    }

 
}
