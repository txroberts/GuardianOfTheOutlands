using UnityEngine;

public class EnemyDeath : MonoBehaviour {

	public int pointsValue = 200;

	void OnTriggerEnter2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		// hit by the player's bullet
		if (layerName.Equals ("Bullet (Player)")) {
			GameObject targetBarrel = GetComponent<EnemyMovement>().getTargetBarrel();

			// if the shot enemy was targeting a barrel, free the barrel for other enemies to target
			if (targetBarrel != null) {
				Barrel barrelScript = targetBarrel.GetComponent<Barrel> ();
				barrelScript.setTargeted (false);
			}
			
			// If the shot enemy was carrying a barrel, drop it
			Transform barrel = transform.FindChild ("Barrel(Clone)");
			if (barrel != null) {
				// Make the barrel targetable by other enemies
				Barrel barrelScript = barrel.GetComponent<Barrel> ();
				barrelScript.setTargeted (false);
				barrelScript.setPickedUp (false);
				
				// give back to the Barrel Spawner object
				barrel.parent = GameObject.Find ("BarrelSpawner").transform;
			}
			
			// Delete the player's bullet
			c.gameObject.SetActive(false);
			
			// Add points to the player's score
			FindObjectOfType<Score>().addPoints(pointsValue);
			
			// Explode the enemy
			GetComponent<Vehicle>().Explosion ();
			
			// Delete the enemy object
			Destroy (gameObject);
			
		}
	}
}