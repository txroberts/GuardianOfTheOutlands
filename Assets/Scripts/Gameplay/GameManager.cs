using UnityEngine;

public class GameManager : MonoBehaviour {

	VehicleFactory vehicleFactory;
	BarrelFactory barrelFactory;

	public int numberOfEnemies = 5;
	public int numberOfBarrels = 5;

	public GameObject enemies, barrels, players;

	void Start () {
		vehicleFactory = GetComponent<VehicleFactory> ();
		barrelFactory = GetComponent<BarrelFactory> ();

		spawnBarrels ();
		spawnPlayer();
		spawnWave(); // spawn first wave of enemies
	}

	void Update () {
		if (enemies.transform.childCount == 0 && numberOfEnemies > 0) { // all enemies in a wave have been destroyed

			// Add points to the player's score for each remaining barrel
			int remainingBarrels = barrels.transform.childCount;
			if (remainingBarrels > 0){
				int barrelPointsValue = barrels.GetComponentInChildren<Barrel>().pointsValue;
				FindObjectOfType<Score>().addPoints(remainingBarrels * barrelPointsValue);

				spawnWave(); // spawn the next wave of enemies
			} else {
				// end game screen
			}
		}

		if (players.transform.childCount == 0){
			spawnPlayer();
		}
	}

	void spawnBarrels () {
		for (int i = 0; i < numberOfBarrels; i++) {
			GameObject barrel = barrelFactory.createBarrel();
			barrel.transform.parent = barrels.transform;
		}
	}

	void spawnPlayer ()	{
		GameObject player = vehicleFactory.createVehicle("player"); // vehicle factory creates a player
		player.transform.parent = players.transform;
		
		// ** Test for spawning another co-op player **
		//Vector3 testSpawn = player.transform.position + transform.forward * 2.0f;
		//Instantiate (player, testSpawn, Quaternion.identity);
	}
	
	void spawnWave () {
		for (int i = 0; i < numberOfEnemies; i++) {
			GameObject enemy = vehicleFactory.createVehicle("enemy");
			enemy.transform.parent = enemies.transform;
		}
	}
}