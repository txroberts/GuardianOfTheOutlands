using UnityEngine;

public class BarrelFactory : MonoBehaviour {
	public GameObject barrelPrefab;
	public float barrelSpawnRadius = 0.8f;

	public GameObject createBarrel () {
		Vector3 center = Vector3.zero;
		return (GameObject) Instantiate (barrelPrefab, randomPositionWithinCircle(center, barrelSpawnRadius), Quaternion.identity);
	}

	Vector3 randomPositionWithinCircle (Vector3 center, float radius) {
		float angle = Random.value * 360;
		Vector3 position;
		position.x = center.x + radius * Mathf.Sin (angle * Mathf.Deg2Rad);
		position.y = center.y + radius * Mathf.Cos (angle * Mathf.Deg2Rad);
		position.z = center.z;
		return position;
	}
}