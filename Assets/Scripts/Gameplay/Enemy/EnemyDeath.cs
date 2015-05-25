using UnityEngine;

public class EnemyDeath : MonoBehaviour {

	public int pointsValue = 200;
	public int hitPoints = 1;
	GameManager gameManager;

	void Start () {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	void OnTriggerEnter2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		// hit by the player's bullet
		if (layerName.Equals ("Bullet (Player)")) {
			hitPoints--;

			if (hitPoints < 1)
				destroyEnemy ();

			// return the player's bullet to the bullet object pool
			c.gameObject.SetActive(false);
		}
	}

	public void destroyEnemy () {
		GameObject targetBarrel = GetComponent<EnemyMovement>().TargetBarrel;
		
		// if the shot enemy was targeting a barrel, free the barrel for other enemies to target
		if (targetBarrel != null) {
			Barrel barrelScript = targetBarrel.GetComponent<Barrel> ();
			barrelScript.Targeted = false;
		}
		
		// If the shot enemy was carrying a barrel, drop it
		Transform barrel = transform.FindChild ("Barrel(Clone)");
		if (barrel != null) {
			// Make the barrel targetable by other enemies
			Barrel barrelScript = barrel.GetComponent<Barrel> ();
			barrelScript.Targeted = false;
			barrelScript.PickedUp = false;
			
			// give back to the Barrels object
			barrel.parent = gameManager.barrels.transform;
		}

		// Add points to the player's score
		gameManager.score.addPoints(pointsValue);
		
		// Explode the enemy
		GetComponent<Vehicle>().Explosion ();
		
		// Delete the enemy object
		Destroy (gameObject);
	}
}