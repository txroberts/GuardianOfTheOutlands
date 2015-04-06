using UnityEngine;

public class GameManager : MonoBehaviour {

	public int numberOfPlayers = 1;
	public int numberOfEnemies = 5;
	public int numberOfBarrels = 5;

	public GameObject enemies, barrels, players;

	void Update () {
		if (players.transform.childCount < 1){
			// Spawn a player at a random position, with a random rotation
			GameObject player = GetComponent<VehicleFactory>().createVehicle("player");

			if (player != null){
				player.transform.parent = players.transform;

				float randX = Random.Range (0f, 0.9f); // random point on the x-axis
				float randY = Random.Range (0f, 0.9f); // random point on the y-axis
				Vector3 spawnPosition = Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));

				player.transform.position = spawnPosition;
				player.transform.rotation = Quaternion.Euler (0, 0, Random.Range (0f, 360f));

				// ** Test for spawning another co-op player **
				//Vector3 testSpawn = player.transform.position + transform.forward * 2.0f;
				//Instantiate (player, testSpawn, Quaternion.identity);
			}
		}
	}
}