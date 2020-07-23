﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 3;
    public float startTimeBtwShots;
    public float attackRange = 5;

    //bullet
    public GameObject projectile;
    public float colDamage = 1;

    //death particles and damage effects
    public GameObject effect;
    public GameObject hurtEffect;
    public SpriteRenderer body;
    public Color hurtColor;

    private Transform player;
    private float dist;
    private float timeBtwShots;

    //audio
    public AudioSource hurtSound;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;

    }
 

    void Update()
    {	

        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= attackRange) {
                // waits 2 seconds between each shot
            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {	
        StartCoroutine(Flash());

        hurtSound.Play();

        health -= damage;


        if (health <= 0)
            Die();

        Instantiate(hurtEffect, transform.position, Quaternion.identity);

        
    }

    void Die()
    {
        //decrement death counter
        GameObject enemies = GameObject.Find("Enemies");
        enemies.GetComponent<OpenPortal>().deathCounter -= 1;

        // death particles
        Vector3 position = new Vector3(transform.position.x, transform.position.y, -8.8f);
        Instantiate(effect, position, Quaternion.identity);

        GetComponent<Patrol>().enabled = false;
        GetComponent<Animator>().SetTrigger("dead");
        this.enabled = false;
    }
    
    IEnumerator Flash() {
        body.color = hurtColor;
        yield return new WaitForSeconds(0.075f);
        body.color = Color.white;
    }


    // player collision damage
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<Player>().TakeDamage(colDamage);
        }
    }

}
