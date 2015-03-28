using UnityEngine;
using System.Collections.Generic;

public class PlayerBulletPool : MonoBehaviour {

	public GameObject bulletPrefab;
	List<GameObject> bullets;
	public int bulletPoolSize = 15;

	void Start () {
		bullets = new List<GameObject> ();
		for (int i = 0; i < bulletPoolSize; i++) {
			GameObject bullet = (GameObject) Instantiate(bulletPrefab);
			bullet.SetActive(false);
			bullet.transform.parent = transform;
			bullets.Add(bullet);
		}
	}
}