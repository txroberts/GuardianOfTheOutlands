using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	// Array of types of enemy
	public GameObject[] enemyTypes;
	public int numEnemies = 0;

	IEnumerator Start()
	{
		if (enemyTypes.Length == 0 || numEnemies == 0) {
			yield break;
		}

		while (true) {
			Vector3 spawnVector;
			float randX, randY;

			for (int i = 0; i < numEnemies; i++){
				// choose an edge to spawn on (0 = top, 1 = right, 2 = bottom, 3 = left)
				int randEdge = Random.Range(0, 4);

				// Random spawn position on the edge of the screen
				switch (randEdge){
				case 0: // top edge
					randX = Random.Range(0f, 1f); // random point on the x-axis
					spawnVector = Camera.main.ViewportToWorldPoint(new Vector3(randX,1,0));
					break;
				case 1: // right edge
					randY = Random.Range(0f, 1f); // random point on the y-axis
					spawnVector = Camera.main.ViewportToWorldPoint(new Vector3(1,randY,0));
					break;
				case 2: // bottom edge
					randX = Random.Range(0f, 1f); // random point on the x-axis
					spawnVector = Camera.main.ViewportToWorldPoint(new Vector3(randX,0,0));
					break;
				case 3: // left edge
					randY = Random.Range(0f, 1f); // random point on the y-axis
					spawnVector = Camera.main.ViewportToWorldPoint(new Vector3(0,randY,0));
					break;
				default: // default bottom-left corner
					spawnVector = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
					break;
				}



				// Spawn a random type of enemy
				int randType = Random.Range(0, enemyTypes.Length);
				GameObject enemy = (GameObject) Instantiate (enemyTypes [randType], spawnVector, Quaternion.identity);

				// Make the enemy a child object of the Enemy Spawner
				enemy.transform.parent = transform;
			}

			// Wait until all of the enemy spawner's child objects have been deleted
			// (all enemies in a wave have been destroyed)
			while (transform.childCount != 0){
				yield return new WaitForEndOfFrame();
			}
		}
	}
}