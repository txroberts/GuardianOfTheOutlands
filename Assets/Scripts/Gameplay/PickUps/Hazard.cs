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

		if (!(layerName.Equals ("Player") || layerName.Equals ("Enemy")))
			return;

		if (layerName.Equals ("Player") && !c.GetComponent<PlayerDeath> ().isInvincible ()) { // collided with a non-invincible player
			if (hazardType.Equals ("Tar Patch")) {
				// temporarily slow the player down
				c.GetComponent<PlayerMovement> ().slowDown (pickUp.effectTime);
			} else if (hazardType.Equals("Oil Slick")) {
				// spin the player to a new (random) direction
				c.GetComponent<PlayerMovement> ().spin ();
			}

			numberOfCollisions++;
		} else  if (layerName.Equals ("Enemy")) { // collided with an enemy
			if (hazardType.Equals ("Tar Patch")) {
				// temporarily slow the enemy down
				c.GetComponent<EnemyMovement> ().slowDown (pickUp.effectTime);
			} else if (hazardType.Equals("Oil Slick")) {
				// spin the enemy to a new (random) direction
				c.GetComponent<EnemyMovement> ().spin ();
			}

			numberOfCollisions++;
		}

		if (numberOfCollisions >= maxCollisions)
			Destroy (gameObject); // destroy the hazard
	}
}