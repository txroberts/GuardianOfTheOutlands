using UnityEngine;

public class PowerUp : MonoBehaviour {

	public string powerUpType;
	PickUp pickUp;

	void Start () {
		pickUp = GetComponent<PickUp>();
	}

	void Update () {
		transform.Rotate (new Vector3 (0, 0, 90) * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D c) {
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name
		
		// quit immediately if it wasn't the player that collided with it
		if (!layerName.Equals ("Player"))
			return;
		
		if (powerUpType.Equals ("Score Multiplier")) {
			FindObjectOfType<Score> ().activateScoreMultiplier (pickUp.effectTime);
		} else if (powerUpType.Equals ("Invincibility")) {
			FindObjectOfType<PlayerDeath>().makeInvincible(pickUp.effectTime);
		}
		
		// destroy the pick-up
		Destroy (gameObject);
	}
}