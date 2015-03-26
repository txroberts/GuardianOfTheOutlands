using UnityEngine;

public class Bullet : MonoBehaviour {

	public int speed = 5;

	void Start () {
		// Move the bullet forwards at its speed
		rigidbody2D.velocity = transform.up.normalized * speed;
	}
}