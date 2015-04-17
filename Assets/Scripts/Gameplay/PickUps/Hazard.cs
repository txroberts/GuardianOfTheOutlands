using UnityEngine;

public class Hazard : MonoBehaviour {

	public string hazardType;
	PickUp pickUp;
	public int maxCollisions = 3;
	int numberOfCollisions;
	
	void Start () {
		pickUp = GetComponent<PickUp>();
		numberOfCollisions = 0;
	}

	void OnTriggerEnter2D (Collider2D c) {
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		if (hazardType.Equals ("Tar Patch")) {
			if (layerName.Equals ("Player") && !c.GetComponent<PlayerDeath> ().isInvincible ()) {
				// temporarily slow the player down
				c.GetComponent<PlayerMovement> ().slowDown (pickUp.effectTime);
				numberOfCollisions++;
			} else if (layerName.Equals ("Enemy")) {
				// temporarily slow the enemy down
				c.GetComponent<EnemyMovement> ().slowDown (pickUp.effectTime);
				numberOfCollisions++;
			}
		}

		if (numberOfCollisions >= maxCollisions) {
			Destroy (gameObject); // destroy the hazard
		}
	}
}