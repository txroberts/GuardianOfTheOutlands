using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour {

	public int deathPenaltyPoints = 1000;

	bool invincible;
	float invincibilityEndTime;
	Slider invincibilitySlider;

	void Start () {
		invincible = false;
		invincibilitySlider = GameObject.Find ("InvincibilityTimer").GetComponent<Slider> ();
		invincibilitySlider.gameObject.SetActive (false);
	}

	void Update () {
		if (invincible) {
			if (Time.time < invincibilityEndTime) {
				invincibilitySlider.value -= 1 * Time.deltaTime;

				// make the slider follow (slightly above) the player
				Vector3 playerPos = Camera.main.WorldToScreenPoint (transform.position);
				invincibilitySlider.transform.position = playerPos + new Vector3(0f, 25f, 0f);
			} else {
				invincible = false;
				invincibilitySlider.gameObject.SetActive (false);
			}
		}
	}

	public void makeInvincible (float invincibilityTime) {
		if (!invincible) {
			invincible = true;
			invincibilitySlider.gameObject.SetActive (true);

			// reset the time slider back to the effect length
			invincibilitySlider.maxValue = invincibilityTime;
			invincibilitySlider.value = invincibilityTime;

			invincibilityEndTime = Time.time + invincibilityTime;
		} else {
			// add extra time if already invincible
			invincibilityEndTime += invincibilityTime;
			invincibilitySlider.maxValue = invincibilitySlider.value + invincibilityTime;
			invincibilitySlider.value += invincibilityTime;
		}
	}

	void OnTriggerEnter2D (Collider2D c){
		// Get the layer name
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		
		// ignore if it wasn't an enemy that collided
		if (!layerName.Equals ("Enemy"))
			return;

		if (!invincible) {
			// Deduct points from the player's score
			FindObjectOfType<Score> ().subtractPoints (deathPenaltyPoints);
		
			// Trigger an explosion
			GetComponent<Vehicle> ().Explosion ();
		
			// Delete the player
			Destroy (gameObject);

			// make active so the next playerDeath script can pick it up
			invincibilitySlider.gameObject.SetActive (true);
		} else {
			c.gameObject.GetComponent<EnemyDeath>().destroyEnemy();
		}
	}

	public bool isInvincible () {
			return invincible;
	}
}