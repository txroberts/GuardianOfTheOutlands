using UnityEngine;

public class GameManager : MonoBehaviour {

	VehicleFactory vehicleFactory;
	BarrelFactory barrelFactory;
	PickUpFactory pickUpFactory;

	public int numberOfEnemies = 5;
	public int numberOfBarrels = 5;
	public int numberOfPowerUps = 3;
	public int numberOfHazards = 3;

	public GameObject enemies, barrels, powerUps, hazards, players;

	public Menu endGameMenu;

	void Start () {
		vehicleFactory = GetComponent<VehicleFactory> ();
		barrelFactory = GetComponent<BarrelFactory> ();
		pickUpFactory = GetComponent<PickUpFactory> ();

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

				FindObjectOfType<WaveCounter>().incrementWaveCounter();
				spawnWave(); // spawn the next wave of enemies
			} else {
				// end game screen
				FindObjectOfType<Timer>().running = false;
				FindObjectOfType<MenuManager>().switchToMenu(endGameMenu);
			}
		}

		if (players.transform.childCount == 0){ // player has died
			spawnPlayer();
		}

		if (powerUps.transform.childCount == 0) { // spawn power-ups
			for (int i = 0; i < numberOfPowerUps; i++) {
				spawnPowerUp();
			}
		}

		if (hazards.transform.childCount == 0) { // spawn hazards
			for (int i = 0; i < numberOfHazards; i++) {
				spawnHazard();
			}
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

		player.transform.position = randomScreenPosition ();
		player.transform.rotation = Quaternion.Euler (0, 0, Random.Range (0f, 360f));
		
		// ** Test for spawning another co-op player **
		//Vector3 testSpawn = player.transform.position + transform.forward * 2.0f;
		//Instantiate (player, testSpawn, Quaternion.identity);
	}
	
	void spawnWave () {
		for (int i = 0; i < numberOfEnemies; i++) {
			GameObject enemy = vehicleFactory.createVehicle("enemy");
			enemy.transform.parent = enemies.transform;
			enemy.transform.position = randomScreenEdgePosition();
			enemy.transform.rotation = Quaternion.identity;
		}
	}

	void spawnPowerUp () {
		GameObject powerUp = pickUpFactory.createPickUp ("Power Up");
		powerUp.transform.parent = powerUps.transform;
		powerUp.transform.position = randomScreenPosition();
	}

	void spawnHazard () {
		GameObject hazard = pickUpFactory.createPickUp ("Hazard");
		hazard.transform.parent = hazards.transform;
		hazard.transform.position = randomScreenPosition();
	}

	Vector3 randomScreenPosition ()	{
		float randX = Random.Range (0f, 0.9f); // random point on the x-axis
		float randY = Random.Range (0f, 0.9f); // random point on the y-axis

		// camera is 10 units above the 'ground', so offset spawn position by 10 in the z axis
		return Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));
	}

	Vector3 randomScreenEdgePosition () {
		Vector3 position;
		float randX, randY;
		
		// choose an edge to spawn on (0 = top, 1 = right, 2 = bottom, 3 = left)
		int randEdge = Random.Range(0, 4);
		
		// Random spawn position on the edge of the screen
		// camera is 10 units above the 'ground', so offset spawn position by 10 in the z axis
		switch (randEdge){
		case 0: // top edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			position = Camera.main.ViewportToWorldPoint(new Vector3(randX,1,10));
			break;
		case 1: // right edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			position = Camera.main.ViewportToWorldPoint(new Vector3(1,randY,10));
			break;
		case 2: // bottom edge
			randX = Random.Range(0f, 1f); // random point on the x-axis
			position = Camera.main.ViewportToWorldPoint(new Vector3(randX,0,10));
			break;
		case 3: // left edge
			randY = Random.Range(0f, 1f); // random point on the y-axis
			position = Camera.main.ViewportToWorldPoint(new Vector3(0,randY,10));
			break;
		default: // default bottom-left corner
			position = Camera.main.ViewportToWorldPoint(new Vector3(0,0,10));
			break;
		}
		
		return position;
	}
}