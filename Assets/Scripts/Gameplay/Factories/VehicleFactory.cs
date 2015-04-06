using UnityEngine;

public class VehicleFactory : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] enemyTypes;

	public GameObject createVehicle (string type) {
		GameObject vehicle = null;

		if (type.Equals ("player")) {
			// create player
			vehicle = (GameObject) Instantiate (playerPrefab);
		}

		return vehicle;
	}
}