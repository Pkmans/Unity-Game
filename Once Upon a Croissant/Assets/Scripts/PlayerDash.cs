﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDash : MonoBehaviour
{   
    //dash stats
    public float dashSpeed;
    public float cooldown;
    public float startDashTime;
    private float dashTime;

    //visuals
    public GameObject dashParticles;
    public GameObject dashIndicator;

    //private variables
    private int direction = 0;
    private Rigidbody2D rb;
    private bool facingRight;
    private bool madeParticles = false;
    private bool canDash = true;

    //audio
    public AudioSource dashSound;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    void Update() {
        facingRight = GetComponent<PlayerMovement>().facingRight;
    }

    public void Dash() {
        if (direction == 0) 
        {
            if(Input.GetKeyDown(KeyCode.LeftShift)) 
            {
                if(!facingRight)
                    direction = 1;
                else if (facingRight)
                    direction = 2;
            }  
        } 
        else 
        {
            if (dashTime <= 0) 
            {
                direction = 0;
                dashTime = startDashTime;
                madeParticles = false;

            }
            else {
                if(direction == 1 && canDash) {
                    if(!madeParticles) {

                        Instantiate(dashParticles, transform.position, dashParticles.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                        madeParticles = true;
                        dashSound.Play();

                    }
                    rb.velocity = Vector2.left * dashSpeed;
                    StartCoroutine(coolDown());
                }
                else if (direction == 2 && canDash) {
                    if(!madeParticles) {

                        Instantiate(dashParticles, transform.position, dashParticles.transform.rotation);
                        madeParticles = true;
                        dashSound.Play();
                        
                    }
                    rb.velocity = Vector2.right * dashSpeed;
                    StartCoroutine(coolDown());
                }

                dashTime -= Time.deltaTime;
            }
        }      
    }



    IEnumerator coolDown() {
        yield return new WaitForSeconds(dashTime);
        canDash = false;
        dashIndicator.GetComponent<Image>().color = new Color (1,1,1, 0.2f);

        yield return new WaitForSeconds(cooldown);
        canDash = true;
        dashIndicator.GetComponent<Image>().color = new Color (1,1,1, 0.8f);

    }
}
