using UnityEngine;

public class Bullet : MonoBehaviour {

	public int speed = 5;

	void OnEnable () {
		// Move the bullet forwards at its speed when it's fired (becomes enabled in the pool)
		rigidbody2D.velocity = transform.up.normalized * speed;
	}
}