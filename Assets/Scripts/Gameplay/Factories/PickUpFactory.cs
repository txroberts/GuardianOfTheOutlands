using UnityEngine;

public class PickUpFactory : MonoBehaviour {

	public GameObject[] powerUpTypes, hazardTypes;

	public GameObject createPickUp (string type) {
		GameObject pickUp = null;

		if (type.Equals ("Power Up")) {
			int randomPowerUp = Random.Range(0, powerUpTypes.Length);
			pickUp = (GameObject) Instantiate (powerUpTypes [randomPowerUp]);
		}

		return pickUp;
	}
}