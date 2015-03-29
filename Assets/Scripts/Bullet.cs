using UnityEngine;

public class Bullet : MonoBehaviour {

	public int speed = 4;

	void OnEnable () {
		// Move the bullet forwards at its speed when it's fired (becomes enabled in the pool)
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}
}