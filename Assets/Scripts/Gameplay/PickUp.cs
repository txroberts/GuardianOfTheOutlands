using UnityEngine;

public class PickUp : MonoBehaviour {

	public string pickUpType;

	void Update () {
		transform.Rotate (new Vector3 (0, 0, 90) * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D c) {
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		// quit immediately if it wasn't the player that collided with it
		if (!layerName.Equals ("Player"))
			return;

		if (pickUpType.Equals ("Score Multiplier")) {
			FindObjectOfType<Score> ().activateScoreMultiplier ();
		} else if (pickUpType.Equals ("Invincibility")) {
			FindObjectOfType<PlayerDeath>().makeInvincible();
		}

		// destroy the pick-up
		Destroy (gameObject);
	}
}