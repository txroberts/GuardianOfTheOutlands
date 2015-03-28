using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Vehicle vehicle;
	Enemy enemy;

	GameObject targetBarrel;
	Barrel targetBarrelScript;

	Vector3 exitPoint, roamTarget;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		enemy = GetComponent<Enemy> ();

		getNewRoamTarget ();
	}

	void FixedUpdate () {
		if (enemy.currentState.Equals ("Move")) {
			MoveToBarrel ();
		} else if (enemy.currentState.Equals ("Escape")) {
			Escape ();
		} else if (enemy.currentState.Equals ("No Target")){
			targetNewBarrel ();
			
			if (targetBarrel != null) {
				enemy.currentState = "Move";
			} else {
				Roam ();
			}
		}
	}

	void getNewRoamTarget(){
		float randX = Random.Range (0f, 1f); // random point on the x-axis
		float randY = Random.Range (0f, 1f); // random point on the y-axis
		roamTarget = Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));
	}

	void Roam () {
		// if the enemy is not close to the waypoint
		if (Vector3.Distance(transform.position, roamTarget) > 0.1f) {
			Vector3 direction = (roamTarget - transform.position).normalized;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
			transform.position += direction * vehicle.movementSpeed * Time.deltaTime;
		} else {
			getNewRoamTarget();
		}
	}
	
	void MoveToBarrel (){
		// move if the target barrel hasn't been picked up by another enemy
		if (!targetBarrelScript.getPickedUp ()) {
			Vector3 direction = (targetBarrel.transform.position - transform.position).normalized;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
			transform.position += direction * vehicle.movementSpeed * Time.deltaTime;
		} else { // target barrel has been picked up by another enemy, switch to roaming mode
			getNewRoamTarget ();
			enemy.currentState = "No Target";
		}
	}

	void targetNewBarrel(){
		GameObject[] barrels = GameObject.FindGameObjectsWithTag ("Barrel");
		
		foreach (GameObject barrel in barrels){
			Barrel barrelScript = barrel.GetComponent<Barrel>();
			
			// barrel is not already being targeted or been picked up
			if (!barrelScript.getTargeted() && !barrelScript.getPickedUp()){
				targetBarrel = barrel;
				targetBarrelScript = targetBarrel.GetComponent<Barrel>();
				
				barrelScript.setTargeted(true);
				return;
			} else{
				targetBarrel = null; // didn't find a targetable barrel
			}
		}
	}

	void Escape () {
		// if the enemy is not close to its exit point
		if (Vector3.Distance (transform.position, exitPoint) > 0.1f) {
			Vector3 direction = (exitPoint - transform.position).normalized;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
			transform.position += direction * vehicle.movementSpeed / 2 * Time.deltaTime;
		} else { // the enemy has reached its exit point, switch to roaming mode
			enemy.currentState = "No Target";
		}
	}

	void getNewExitPoint(){
		float randX, randY;
		
		// choose an edge to exit on (0 = top, 1 = right, 2 = bottom, 3 = left)
		int randEdge = Random.Range (0, 4);
		
		// Random exit position on that edge of the screen
		// ViewportToWorldPoint camera (0,0) is bottom-left, (1,1) is top-right
		switch (randEdge){
		case 0: // top edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(randX,1,10));
			break;
		case 1: // right edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(1,randY,10));
			break;
		case 2: // bottom edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(randX,0,10));
			break;
		case 3: // left edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(0,randY,10));
			break;
		default: // default bottom-left corner
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(0,0,10));
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		// hit by the player's bullet
		if (layerName.Equals ("Bullet (Player)")) {
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
			FindObjectOfType<Score>().addPoints(enemy.pointsValue);
		
			// Explode the enemy
			vehicle.Explosion ();
		
			// Delete the enemy object
			Destroy (gameObject);

		} else if (layerName.Equals ("Barrel")){ // collided with a barrel
			Barrel barrelScript = c.GetComponent<Barrel> ();

			// Only pick up a barrel if not already carrying one and the barrel isn't being carried by another enemy
			if (transform.FindChild ("Barrel(Clone)") == null && !barrelScript.getPickedUp()) {
				// If the barrel wasn't the enemy's intended barrel, free the intended barrel for other enemies
				if (targetBarrel != null && !targetBarrel.Equals (gameObject)) {
					Barrel targetBarrelScript = targetBarrel.GetComponent<Barrel> ();
					targetBarrelScript.setTargeted (false);
				}
				
				// 'pick up the barrel' (make it a child of the enemy that collided with it)
				c.transform.parent = transform;
				barrelScript.setTargeted(false);
				barrelScript.setPickedUp(true);

				// get a place to escape to, switch to escape mode
				getNewExitPoint ();
				enemy.setCurrentState ("Escape");
			}
		}
	}

	public GameObject getTargetBarrel(){
		return targetBarrel;
	}
}