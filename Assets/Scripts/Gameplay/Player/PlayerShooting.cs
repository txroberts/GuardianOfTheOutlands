using UnityEngine;

public class PlayerShooting : MonoBehaviour {

	Animator anim;
	AudioSource playerAudioSource;

	PlayerBulletPool bulletPool;

	// Vehicle shooting timers
	public float shootDelay = 0.3333f;
	float nextShot;

	GameManager gameManager;

	void Start () {
		anim = GetComponent<Animator> ();
		playerAudioSource = GetComponent<AudioSource> ();

		bulletPool = GameObject.Find ("PlayerBulletPool").GetComponent<PlayerBulletPool> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void Update () {
		if (Input.GetMouseButton (0) || Input.GetKey("space")) {
			// Shoot a bullet
			if (Time.time >= nextShot && Time.timeScale == 1f && gameManager.gameRunning) {
				if (bulletPool.fireBullet(transform)) { // true if a bullet was fired
					nextShot = Time.time + shootDelay; // the next time when a bullet can be fired
					anim.SetBool ("IsShooting", true); // trigger the shooting animation
					playerAudioSource.Play ();
				}
			}
		} else {
			anim.SetBool ("IsShooting", false); // disable the shooting animation
		}
	}
}