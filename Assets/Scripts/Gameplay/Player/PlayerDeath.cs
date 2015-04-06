using UnityEngine;

public class PlayerDeath : MonoBehaviour {

	public int deathPenaltyPoints = 1000;

	void OnTriggerEnter2D (Collider2D c){
		// Get the layer name
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		
		// ignore if it wasn't an enemy that collided
		if (!layerName.Equals ("Enemy"))
			return;
		
		// Deduct points from the player's score
		FindObjectOfType<Score>().subtractPoints(deathPenaltyPoints);
		
		// Trigger an explosion
		GetComponent<Vehicle> ().Explosion ();
		
		// Delete the player
		Destroy (gameObject);
	}
}