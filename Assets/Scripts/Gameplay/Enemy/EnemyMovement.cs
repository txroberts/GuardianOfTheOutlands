using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Vehicle vehicle;

	bool slowedDown;
	float slowedDownEndTime;

	public string currentState;

	GameObject targetBarrel;
	Barrel targetBarrelScript;

	Animator damageImageAnimator;

	Vector3 exitPoint, roamTarget;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		slowedDown = false;

		currentState = "No Target";

		damageImageAnimator = GameObject.Find ("DamageImage").GetComponent<Animator> ();

		getNewRoamTarget ();
	}

	void Update () {
		if (slowedDown && Time.time >= slowedDownEndTime) {
			vehicle.movementSpeed *= 3; // set the enemy's movement speed back to normal
			slowedDown = false;
		}

		if (currentState.Equals ("Move")) {
			MoveToBarrel ();
		} else if (currentState.Equals ("Escape")) {
			Escape ();
		} else if (currentState.Equals ("No Target")){
			targetNewBarrel ();
			
			if (targetBarrel != null) {
				currentState = "Move";
			} else {
				Roam ();
			}
		}
	}

	public void slowDown (float slowedDownTime)	{
		if (!slowedDown) {
			slowedDown = true;
			vehicle.movementSpeed /= 3; // third the enemy's movement speed
		}
		
		slowedDownEndTime = Time.time + slowedDownTime; // reset the timer
	}

	void getNewRoamTarget(){
		float randX = Random.Range (0f, 0.9f); // random point on the x-axis
		float randY = Random.Range (0f, 0.9f); // random point on the y-axis
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
		if (!targetBarrelScript.PickedUp) {
			Vector3 direction = (targetBarrel.transform.position - transform.position).normalized;
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, angle - 90);
			transform.position += direction * vehicle.movementSpeed * Time.deltaTime;
		} else { // target barrel has been picked up by another enemy, switch to roaming mode
			getNewRoamTarget ();
			currentState = "No Target";
		}
	}

	void targetNewBarrel(){
		GameObject[] barrels = GameObject.FindGameObjectsWithTag ("Barrel");
		
		foreach (GameObject barrel in barrels){
			Barrel barrelScript = barrel.GetComponent<Barrel>();
			
			// barrel is not already being targeted or been picked up
			if (!barrelScript.Targeted && !barrelScript.PickedUp){
				targetBarrel = barrel;
				targetBarrelScript = targetBarrel.GetComponent<Barrel>();
				
				barrelScript.Targeted = true;
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
			if (transform.FindChild("Barrel(Clone)") != null){
				Destroy (transform.FindChild("Barrel(Clone)").gameObject);
				damageImageAnimator.SetTrigger("BarrelStolen");
				currentState = "No Target";
			}
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
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(randX,0.99f,10));
			break;
		case 1: // right edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.99f,randY,10));
			break;
		case 2: // bottom edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(randX,0.01f,10));
			break;
		case 3: // left edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.01f,randY,10));
			break;
		default: // default bottom-left corner
			exitPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.01f,0.01f,10));
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name

		if (layerName.Equals ("Barrel")){ // collided with a barrel
			Barrel barrelScript = c.GetComponent<Barrel> ();

			// Only pick up a barrel if not already carrying one and the barrel isn't being carried by another enemy
			if (transform.FindChild ("Barrel(Clone)") == null && !barrelScript.PickedUp) {
				// If the barrel wasn't the enemy's intended barrel, free the intended barrel for other enemies
				if (targetBarrel != null && !targetBarrel.Equals (gameObject)) {
					Barrel targetBarrelScript = targetBarrel.GetComponent<Barrel> ();
					targetBarrelScript.Targeted = false;
				}
				
				// 'pick up the barrel' (make it a child of the enemy that collided with it)
				c.transform.parent = transform;
				barrelScript.Targeted = false;
				barrelScript.PickedUp = true;

				// get a place to escape to, switch to escape mode
				getNewExitPoint ();
				currentState = "Escape";
			}
		}
	}

	public GameObject getTargetBarrel() {
		return targetBarrel;
	}
}