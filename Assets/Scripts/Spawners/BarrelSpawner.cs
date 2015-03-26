using UnityEngine;

public class BarrelSpawner : MonoBehaviour {
	public GameObject barrelPrefab;
	public int numBarrels = 1;
	public float barrelSpawnRadius = 0.3f;

	void Start(){
		SpawnBarrels ();
	}

	void SpawnBarrels(){
		Vector3 center = Vector3.zero;
		
		for (int i = 0; i < numBarrels; i++) {
			Vector3 position = RandomCircle(center, barrelSpawnRadius);
			GameObject barrel = (GameObject) Instantiate(barrelPrefab, position, Quaternion.identity);
			barrel.transform.parent = transform;
		}
	}

	Vector3 RandomCircle (Vector3 center, float radius){
		float angle = Random.value * 360;
		Vector3 position;
		position.x = center.x + radius * Mathf.Sin (angle * Mathf.Deg2Rad);
		position.y = center.y + radius * Mathf.Cos (angle * Mathf.Deg2Rad);
		position.z = center.z;
		return position;
	}
}