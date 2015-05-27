using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	Vehicle vehicle;

	bool slowedDown;
	float slowedDownEndTime;
	
	void Start (){
		vehicle = GetComponent<Vehicle> ();
		slowedDown = false;
	}
	
	void Update(){
		if (slowedDown && Time.time >= slowedDownEndTime) {
			vehicle.movementSpeed *= 3; // set the player's movement speed back to normal
			slowedDown = false;
		}

		// get horizontal and vertical axes
		float translation = Input.GetAxis ("Vertical");
		float rotation = Input.GetAxis ("Horizontal");
		translation = translation * vehicle.movementSpeed * Time.deltaTime;
		rotation = rotation * -vehicle.rotationSpeed * Time.deltaTime;
	
		// rotate and move the player
		transform.Rotate (0, 0, rotation);
		transform.Translate (0, translation, 0);
	}

	public void slowDown (float slowedDownTime)	{
		if (!slowedDown) {
			slowedDown = true;
			vehicle.movementSpeed /= 3; // third the player's movement speed
		}

		slowedDownEndTime = Time.time + slowedDownTime; // reset the timer
	}

	public void spin () {
		// give the player a new random rotation
		transform.rotation = Quaternion.Euler (0, 0, Random.Range (0f, 360f));
	}
}