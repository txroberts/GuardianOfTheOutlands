using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	Vehicle vehicle;

	bool slowedDown;
	float slowedDownEndTime;

	public string currentState;

	Barrel[] barrels;
	GameObject targetBarrel;

	Animator damageImageAnimator;

	Vector3 movementTarget;
	float newRoamTime;
	float currentSpeed;
	float slowDownRadius, arrivedRadius;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		slowedDown = false;

		currentState = "No Target";
		movementTarget = newRoamTarget ();
		currentSpeed = vehicle.movementSpeed;
		slowDownRadius = 0.8f;
		arrivedRadius = 0.1f;

		barrels = GameObject.Find ("Game Manager").GetComponent<GameManager> ().barrels.GetComponentsInChildren<Barrel> ();

		damageImageAnimator = GameObject.Find ("DamageImage").GetComponent<Animator> ();

		// point the enemy as its initial target
		Vector3 directionToTarget = movementTarget - transform.position;
		Quaternion angleToTarget = Quaternion.FromToRotation (Vector3.up, directionToTarget);
		angleToTarget.x = angleToTarget.y = 0; // only use the z-axis angle
		transform.rotation = angleToTarget;
	}

	void Update () {
		if (currentState.Equals ("Move To Barrel")) {
			checkTargetBarrel ();
		} else if (currentState.Equals ("Escape")) {
			checkEscape ();
		} else if (currentState.Equals ("No Target")) {
			if (Time.time >= newRoamTime) // get a new roam target at set intervals
				movementTarget = newRoamTarget ();

			targetNewBarrel ();

			if (targetBarrel != null) { // found a targetable barrel
				currentState = "Move To Barrel";
				movementTarget = targetBarrel.transform.position;
			} else {
				float distanceToTarget = Vector3.Distance (transform.position, movementTarget);

				if (distanceToTarget > arrivedRadius) {
					Move (); // move to the target
				} else {
					movementTarget = newRoamTarget ();
				}
			}
		}
	}

	void Move () {
		if (slowedDown && Time.time >= slowedDownEndTime) {
			vehicle.movementSpeed *= 3; // set the enemy's movement speed back to normal
			slowedDown = false;
		}
		
		Vector3 directionToTarget = (movementTarget - transform.position).normalized;

		Quaternion angleToTarget = Quaternion.FromToRotation (Vector3.up, directionToTarget);
		angleToTarget.x = angleToTarget.y = 0; // only use the z-axis angle

		transform.rotation = Quaternion.Slerp (transform.rotation, angleToTarget, vehicle.rotationSpeed * Time.deltaTime);

		float distanceToTarget = Vector3.Distance (transform.position, movementTarget);;

		if (distanceToTarget > 0.8f) { // outside of the 'nearby' radius
			// move enemy at full speed
			currentSpeed = vehicle.movementSpeed;
		} else if (distanceToTarget <= slowDownRadius && distanceToTarget > arrivedRadius) { // inside the 'nearby' radius but outside the 'arrived' radius
			// slow the enemy down as it approaches its target to prevent orbiting
			currentSpeed = vehicle.movementSpeed * (distanceToTarget / slowDownRadius);
		}

		if (!currentState.Equals ("Escape"))
			transform.Translate (Vector3.up * currentSpeed * Time.deltaTime);
		else
			transform.Translate (Vector3.up * (currentSpeed / 2) * Time.deltaTime);
	}

	void checkTargetBarrel () {
		if (!targetBarrel.GetComponent<Barrel> ().PickedUp) {
			Move (); // move to the target
		} else {
			// target barrel has been picked up by another enemy
			currentState = "No Target";
			movementTarget = newRoamTarget ();
		}
	}

	void checkEscape ()	{
		float distanceToTarget = Vector3.Distance (transform.position, movementTarget);

		if (distanceToTarget > arrivedRadius) {
			Move (); // move to the target
		} else {
			// enemy has reached its exit point, switch to roaming mode
			Transform barrel = transform.FindChild("Barrel(Clone)");
			
			if (barrel != null){
				Destroy (barrel.gameObject);
				damageImageAnimator.SetTrigger("BarrelStolen");
				
				currentState = "No Target";
				movementTarget = newRoamTarget ();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // get the layer name
		
		if (layerName.Equals ("Barrel")){ // collided with a barrel
			Barrel barrel = c.GetComponent<Barrel> ();
			
			// Only pick up a barrel if not already carrying one and the barrel isn't being carried by another enemy
			if (transform.FindChild ("Barrel(Clone)") == null && !barrel.PickedUp) {
				// If the barrel wasn't the enemy's intended barrel, free the intended barrel for other enemies
				if (targetBarrel != null && !targetBarrel.Equals (gameObject)) {
					Barrel targetBarrelScript = targetBarrel.GetComponent<Barrel> ();
					targetBarrelScript.Targeted = false;
				}
				
				// 'pick up' the barrel (make it a child of the enemy that collided with it)
				c.transform.parent = transform;
				barrel.Targeted = false;
				barrel.PickedUp = true;
				
				// get a place to escape to and switch to escape mode
				movementTarget = newExitPoint ();
				currentState = "Escape";
			}
		}
	}
	
	void targetNewBarrel(){
		foreach (Barrel barrel in barrels){
			// barrel is not already being targeted or been picked up
			if (!barrel.Targeted && !barrel.PickedUp){
				targetBarrel = barrel.gameObject;
				
				barrel.Targeted = true;
				return;
			} else{
				targetBarrel = null; // didn't find a targetable barrel
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

	Vector3 newRoamTarget() {
		newRoamTime = Time.time + 5f; // get a new roam target every 5 seconds whilst roaming

		float randX = Random.Range (0f, 0.9f); // random point on the x-axis
		float randY = Random.Range (0f, 0.9f); // random point on the y-axis
		return Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));
	}

	Vector3 newExitPoint(){
		// choose an edge to exit on (0 = top, 1 = right, 2 = bottom, 3 = left)
		int randEdge = Random.Range (0, 4);

		float randX, randY;
		
		// Random exit position on that edge of the screen
		// ViewportToWorldPoint camera: (0,0) is bottom-left, (1,1) is top-right
		switch (randEdge){
		case 0: // top edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			return Camera.main.ViewportToWorldPoint(new Vector3(randX,0.99f,10));
		case 1: // right edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			return Camera.main.ViewportToWorldPoint(new Vector3(0.99f,randY,10));
		case 2: // bottom edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			return Camera.main.ViewportToWorldPoint(new Vector3(randX,0.01f,10));
		case 3: // left edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			return Camera.main.ViewportToWorldPoint(new Vector3(0.01f,randY,10));
		default: // default bottom-left corner
			return Camera.main.ViewportToWorldPoint(new Vector3(0.01f,0.01f,10));
		}
	}

	public GameObject TargetBarrel {
		get { return targetBarrel; }
	}
}