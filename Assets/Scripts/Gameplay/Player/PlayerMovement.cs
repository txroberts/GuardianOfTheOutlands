using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	Vehicle vehicle;
	
	void Start (){
		vehicle = GetComponent<Vehicle> ();
	}
	
	void FixedUpdate(){
		// Get horizontal and vertical axes
		float translation = Input.GetAxis ("Vertical");
		float rotation = Input.GetAxis ("Horizontal");
		translation = translation * vehicle.movementSpeed * Time.deltaTime;
		rotation = rotation * -vehicle.rotationSpeed * Time.deltaTime;
		
		// Rotate and move the player
		transform.Rotate (0, 0, rotation);
		transform.Translate (0, translation, 0);
		
		// Move player using physics
		//rigidbody2D.AddForce (transform.up * translation);
	}
}