using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;

	IEnumerator Start () {

		while (true) {
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));

			// Spawn a player at a random position, with a random rotation
			GameObject player = (GameObject)Instantiate (playerPrefab, spawnPosition, Quaternion.Euler (0, 0, Random.Range (0f, 360f)));

			player.transform.parent = transform;

			// ** Test for spawning another co-op player **
			//Vector3 testSpawn = player.transform.position + transform.forward * 2.0f;
			//Instantiate (player, testSpawn, Quaternion.identity);

			while (transform.childCount == 1){
				yield return new WaitForEndOfFrame();
			}
		}
	}
}