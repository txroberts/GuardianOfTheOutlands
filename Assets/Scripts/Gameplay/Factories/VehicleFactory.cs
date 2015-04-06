using UnityEngine;

public class VehicleFactory : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] enemyTypes;

	public GameObject createVehicle (string type) {
		GameObject vehicle = null;

		if (type.Equals("enemy")) {
			// create a random type of enemy
			int randomType = Random.Range(0, enemyTypes.Length);
			vehicle = (GameObject) Instantiate (enemyTypes [randomType]);
		} else if (type.Equals ("player")) {
			// create player
			vehicle = (GameObject) Instantiate (playerPrefab);
		}

		return vehicle;
	}
}