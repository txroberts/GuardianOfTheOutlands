using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Vehicle vehicle;
	Enemy enemy;

	GameObject targetBarrel;
	Barrel targetBarrelScript;

	Vector3 exitPoint;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		enemy = GetComponent<Enemy> ();

		getExitPoint ();
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
				enemy.currentState = "No Target";
			}
		}
	}
	
	void MoveToBarrel (){
		if (!targetBarrelScript.getPickedUp ()) {
			Vector3 direction = (targetBarrel.transform.position - transform.position).normalized;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
			transform.position += direction * vehicle.movementSpeed * Time.deltaTime;
		} else {
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
		Vector3 direction = (exitPoint - transform.position).normalized;
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
		
		transform.position += direction * vehicle.movementSpeed/2 * Time.deltaTime;
	}

	void getExitPoint(){
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
		// Get the layer name
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		
		if (layerName.Equals ("Bullet (Player)")) {
			Transform barrel = transform.FindChild ("Barrel(Clone)");
			
			// if the enemy was targeting a barrel, free the barrel for other enemies to target
			if (targetBarrel != null){
				Barrel barrelScript = targetBarrel.GetComponent<Barrel>();
				barrelScript.setTargeted(false);
			}
			
			// If the enemy was carrying a barrel, drop it
			if (barrel != null){
				// Make the barrel targetable by other enemies
				Barrel barrelScript = barrel.GetComponent<Barrel>();
				barrelScript.setTargeted(false);
				barrelScript.setPickedUp(false);
				
				//barrel.parent = null; // detach it from the shot enemy
				barrel.parent = GameObject.Find ("BarrelSpawner").transform;
			}
			
			// Delete the bullet
			Destroy (c.gameObject);
			
			// Explode the enemy
			vehicle.Explosion ();
			
			// Delete the enemy object
			Destroy (gameObject);
		}
	}

	public GameObject getTargetBarrel(){
		return targetBarrel;
	}
}