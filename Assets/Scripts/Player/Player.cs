﻿using UnityEngine;

public class Player : MonoBehaviour {

	Vehicle vehicle;
	Animator anim;

	// Vehicle shooting timers
	public float shootDelay = 0.5f;
	private float nextShot;

	public GameObject bullet;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate(){
		if (Input.GetMouseButton (0) || Input.GetKey("space")) {
			Shoot ();
		} else {
			anim.SetBool ("IsShooting", false); // disable the shooting animation
		}
	}

	// Shoot a bullet
	public void Shoot (){
		if (Time.time >= nextShot) {
			Instantiate (bullet, transform.position, transform.rotation);
			nextShot = Time.time + shootDelay; // the next time when a bullet can be fired
			anim.SetBool ("IsShooting", true); // trigger the shooting animation
		}
	}

	void OnTriggerEnter2D (Collider2D c){
		// Get the layer name
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		
		// ignore if it wasn't an enemy that collided
		if (!layerName.Equals ("Enemy"))
			return;

		// Trigger an explosion
		vehicle.Explosion ();

		// Delete the player
		Destroy (gameObject);
	}
}