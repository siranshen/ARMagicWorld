﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public float damageInterval = 0.3f;
	public GameObject healEffect;
	public Transform healEffectTransform;



    Animation anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;
	bool damaged;
	float damageTimer;
	int regularSwordDamage=10;
	int powerfulSwordDamage=20;

    void Awake ()
    {
        anim = GetComponent <Animation> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
		damageTimer = damageInterval;
    }


    void Update ()
    {
		damageTimer += Time.deltaTime;
        /*if(damaged)
            damageImage.color = flashColour;
        else
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        damaged = false;*/
    }


    public void TakeDamage (int amount)
    {
		if (isDead)
			return;
        damaged = true;
        currentHealth -= amount;
        //healthSlider.value = currentHealth;
        playerAudio.Play ();
		if (!anim.IsPlaying ("Wound")) {
			anim.Play ("Wound");
			Debug.Log("Wounded");
		}
        if(currentHealth <= 0 && !isDead)
            Death ();
    }


    void Death ()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

		anim.Play ("HitAway");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }

	public void OnTriggerEnter(Collider col){
		if(( col.gameObject.CompareTag ("RegularSword") || col.gameObject.CompareTag ("PowerfulSword") ) && damageTimer>damageInterval && col.transform.root.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_slice")){
			if (col.gameObject.CompareTag ("RegularSword")) {
				TakeDamage (regularSwordDamage);
			} else if (col.gameObject.CompareTag ("PowerfulSword")) {
				TakeDamage (powerfulSwordDamage);
			}
			damageTimer = 0f;
		}
	}

	public void HealEffect(){
		GameObject effect = Instantiate (healEffect, healEffectTransform.position, healEffectTransform.rotation) as GameObject;
		effect.transform.parent = healEffectTransform;
	}
}