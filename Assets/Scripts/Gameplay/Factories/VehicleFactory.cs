using UnityEngine;

public class VehicleFactory : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] enemyTypes;

	public GameObject createVehicle (string type) {
		GameObject vehicle = null;

		if (type.Equals("enemy")) {
			// create a random type of enemy
			int randomType = Random.Range(0, enemyTypes.Length);
			vehicle = (GameObject) Instantiate (enemyTypes [randomType], randomScreenEdgePosition(), Quaternion.identity);
		} else if (type.Equals ("player")) {
			// create player
			vehicle = (GameObject) Instantiate (playerPrefab, randomScreenPosition (), Quaternion.Euler (0, 0, Random.Range (0f, 360f)));
		}

		return vehicle;
	}

	Vector3 randomScreenPosition ()	{
		float randX = Random.Range (0f, 0.9f); // random point on the x-axis
		float randY = Random.Range (0f, 0.9f); // random point on the y-axis
		return Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 10));
	}

	Vector3 randomScreenEdgePosition () {
		Vector3 position;
		float randX, randY;
		
		// choose an edge to spawn on (0 = top, 1 = right, 2 = bottom, 3 = left)
		int randEdge = Random.Range(0, 4);
		
		// Random spawn position on the edge of the screen
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