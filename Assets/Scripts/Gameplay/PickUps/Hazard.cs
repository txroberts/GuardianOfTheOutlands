using UnityEngine;

public class Hazard : MonoBehaviour {

	public string hazardType;
	PickUp pickUp;
	
	void Start () {
		pickUp = GetComponent<PickUp>();
	}

	void OnTriggerEnter2D (Collider2D c) {
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		if (hazardType.Equals ("Tar Patch")) {
			if (layerName.Equals ("Player") && !c.GetComponent<PlayerDeath> ().isInvincible ()) {
				// temporarily slow the player down
				c.GetComponent<PlayerMovement> ().slowDown (pickUp.effectTime);
			} else if (layerName.Equals ("Enemy")) {
				// temporarily slow the enemy down
				c.GetComponent<EnemyMovement> ().slowDown (pickUp.effectTime);
			}
		}
		
		// destroy the pick-up
		Destroy (gameObject);
	}
}