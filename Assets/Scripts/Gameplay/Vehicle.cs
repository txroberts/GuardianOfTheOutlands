using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class Vehicle : MonoBehaviour {

	// Vehicle movement/rotation speeds
	public float movementSpeed = 3f;
	public float rotationSpeed = 200f;

	public GameObject explosion;

	public void Explosion(){
		Instantiate (explosion, transform.position, transform.rotation);
	}
}