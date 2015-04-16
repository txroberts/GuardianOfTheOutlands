using UnityEngine;

public class PickUpFactory : MonoBehaviour {

	public GameObject[] powerUpTypes, hazardTypes;

	int powerUpsTotalWeight, hazardsTotalWeight;

	void Start () {
		powerUpsTotalWeight = 0;
		for (int i = 0; i < powerUpTypes.Length; i++) {
			powerUpsTotalWeight += powerUpTypes[i].GetComponent<PickUp>().spawnProbability;
		}

		hazardsTotalWeight = 0;
		for (int i = 0; i < hazardTypes.Length; i++) {
			hazardsTotalWeight += hazardTypes[i].GetComponent<PickUp>().spawnProbability;
		}
	}

	public GameObject createPickUp (string type) {
		if (type.Equals ("Power Up")) {
			return weightedRandomType (powerUpTypes, powerUpsTotalWeight);
		} else if (type.Equals ("Hazard")) {
			return weightedRandomType (hazardTypes, hazardsTotalWeight);
		}

		return null; // no pick-up was created
	}

	GameObject weightedRandomType (GameObject[] types, int totalWeight) {
		int randomWeight = Random.Range(0, totalWeight);

		int pickUpWeight;
		for (int i = 0; i < types.Length; i++) {
			pickUpWeight = types[i].GetComponent<PickUp>().spawnProbability;

			if (randomWeight < pickUpWeight)
				return (GameObject) Instantiate (types [i]);

			randomWeight -= pickUpWeight;
		}

		return null;
	}
}