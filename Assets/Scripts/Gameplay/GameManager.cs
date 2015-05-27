using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool gameRunning;

	VehicleFactory vehicleFactory;
	BarrelFactory barrelFactory;
	PickUpFactory pickUpFactory;

	public int numberOfEnemies = 5;
	public int numberOfBarrels = 5;
	public int numberOfPowerUps = 5;
	public int numberOfHazards = 7;

	public GameObject enemies, barrels, powerUps, hazards, players;

	public float enemyMovementSpeedModifier = 0.2f;

	public Score score;
	public WaveCounter waveCounter;

	public Menu endGameMenu;
	public GameObject pauseMenu;

	void Start () {
		gameRunning = true;

		vehicleFactory = GetComponent<VehicleFactory> ();
		barrelFactory = GetComponent<BarrelFactory> ();
		pickUpFactory = GetComponent<PickUpFactory> ();

		pauseMenu.SetActive (false);
		RectTransform pauseMenuRectTransform = pauseMenu.GetComponent<RectTransform> ();
		pauseMenuRectTransform.offsetMax = pauseMenuRectTransform.offsetMin = new Vector2 (0, 0);

		spawnBarrels ();
		spawnPlayer();
		spawnWave(); // spawn first wave of enemies
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && gameRunning) {
			if (Time.timeScale == 1f) {
				pauseMenu.SetActive(true);
				setTimeScale(0f);
			}
			else {
				setTimeScale(1f);
				pauseMenu.SetActive(false);
			}
		}

		//int remainingBarrels = barrels.transform.childCount;
		int remainingBarrels = GameObject.FindObjectsOfType<Barrel>().Length;

		if (gameRunning && remainingBarrels == 0) {
			// end game screen
			FindObjectOfType<Timer>().running = false;
			FindObjectOfType<MenuManager>().switchToMenu(endGameMenu);
			
			string gameTime = GameObject.Find("Timer").GetComponent<Text>().text;
			
			// update the leaderboard
			updateHighScores (score.score, waveCounter.waveCount, gameTime);
			
			gameRunning = false;

			if (enemies.transform.childCount > 0) {
				for (int i = 0; i < enemies.transform.childCount; i++) {
					Destroy(enemies.transform.GetChild(i).gameObject);
				}
			}
		}

		if (enemies.transform.childCount == 0 && numberOfEnemies > 0) { // all enemies in a wave have been destroyed
			// Add points to the player's score for each remaining barrel
			if (remainingBarrels > 0){
				int barrelPointsValue = barrels.GetComponentInChildren<Barrel>().pointsValue;
				score.addPoints(remainingBarrels * barrelPointsValue);

				waveCounter.incrementWaveCounter();
				spawnWave(); // spawn the next wave of enemies
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

		int currentNumHazards = hazards.transform.childCount;
		if (currentNumHazards < numberOfHazards) { // spawn hazards
			for (int i = 0; i < numberOfHazards - currentNumHazards; i++) {
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

			// increase enemy movement speed if barrels have already been stolen
			int barrelsStolen = numberOfBarrels - barrels.transform.childCount;
			enemy.GetComponent<Vehicle>().movementSpeed += enemyMovementSpeedModifier * barrelsStolen;
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

	public void increaseEnemyMovementSpeed () {
		for (int i = 0; i < enemies.transform.childCount; i++) {
			Vehicle enemyVehicle = enemies.transform.GetChild(i).GetComponent<Vehicle>();
			enemyVehicle.movementSpeed += enemyMovementSpeedModifier;
		}
	}
	
	public void setTimeScale (float newTimeScale) {
		Time.timeScale = newTimeScale;
	}

	void updateHighScores (int newScore, int newWaves, string newGameTime) {
		// timestamp is stored in PlayerPrefs as a string
		string newTimeStamp = System.DateTime.Now.ToBinary ().ToString ();
		// System.DateTime recovered = System.DateTime.FromBinary (System.Convert.ToInt64 (storedTimeStamp));
		string oldTimeStamp;
		
		int oldScore, oldWaves;
		string oldGameTime;
		
		for (int i = 0; i < 10; i++) {
			if (PlayerPrefs.HasKey ("HS_score_" + i)) {
				if (newScore > PlayerPrefs.GetInt ("HS_score_" + i)) { // new score is greater than this leaderboard entry
					// remember the current entry (about to be overwritten) at this position
					oldScore = PlayerPrefs.GetInt ("HS_score_" + i);
					oldWaves = PlayerPrefs.GetInt ("HS_waves_" + i);
					oldGameTime = PlayerPrefs.GetString ("HS_gameTime_" + i);
					oldTimeStamp = PlayerPrefs.GetString ("HS_timestamp_" + i);
					
					// overwrite with the new score
					PlayerPrefs.SetInt ("HS_score_" + i, newScore);
					PlayerPrefs.SetInt ("HS_waves_" + i, newWaves);
					PlayerPrefs.SetString ("HS_gameTime_" + i, newGameTime);
					PlayerPrefs.SetString ("HS_timestamp_" + i, newTimeStamp);
					
					// the replaced entry becomes the 'new' entry for the next loop iteration
					// this has the effect of shifting all of the proceeding leaderboard entries down a position
					newScore = oldScore;
					newWaves = oldWaves;
					newGameTime = oldGameTime;
					newTimeStamp = oldTimeStamp;
				}
			} else {
				// no high score at this position, insert a new one
				PlayerPrefs.SetInt ("HS_score_" + i, newScore);
				PlayerPrefs.SetInt ("HS_waves_" + i, newWaves);
				PlayerPrefs.SetString ("HS_gameTime_" + i, newGameTime);
				PlayerPrefs.SetString ("HS_timestamp_" + i, newTimeStamp);
				
				// show the proceeding leaderboard entries as blank
				newScore = 0;
				newWaves = 0;
				newGameTime = "--:--:--";
				newTimeStamp = "--/--/-- --:--:--";
			}
		}
		
		PlayerPrefs.Save ();
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