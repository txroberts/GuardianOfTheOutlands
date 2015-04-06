using UnityEngine;

public class PlayerShooting : MonoBehaviour {

	Animator anim;
	PlayerBulletPool bulletPool;

	// Vehicle shooting timers
	public float shootDelay = 0.3333f;
	private float nextShot;

	void Start () {
		anim = GetComponent<Animator> ();
		bulletPool = GameObject.Find ("PlayerBulletPool").GetComponent<PlayerBulletPool> ();
	}

	void Update () {
		if (Input.GetMouseButton (0) || Input.GetKey("space")) {
			// Shoot a bullet
			if (Time.time >= nextShot) {
				if (bulletPool.fireBullet(transform)) { // true if a bullet was fired
					nextShot = Time.time + shootDelay; // the next time when a bullet can be fired
					anim.SetBool ("IsShooting", true); // trigger the shooting animation
				}
			}
		} else {
			anim.SetBool ("IsShooting", false); // disable the shooting animation
		}
	}
}