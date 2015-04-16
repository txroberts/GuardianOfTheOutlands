using UnityEngine;

public class Hazard : MonoBehaviour {

	public string hazardType;
	PickUp pickUp;
	
	void Start () {
		pickUp = GetComponent<PickUp>();
	}

	void OnTriggerEnter2D (Collider2D c) {
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name
		
		// quit immediately if it wasn't the player that collided with it
		if (!layerName.Equals ("Player"))
			return;

		if (!FindObjectOfType<PlayerDeath> ().isInvincible ()) {
			if (hazardType.Equals ("Tar Patch")) {
				// temporarily slow the player down
				FindObjectOfType<PlayerMovement> ().slowDown (pickUp.effectTime);
			}
		}
		
		// destroy the pick-up
		Destroy (gameObject);
	}
}