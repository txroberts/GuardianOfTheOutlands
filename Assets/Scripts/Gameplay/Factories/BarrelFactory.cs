using UnityEngine;

public class BarrelFactory : MonoBehaviour {
	public GameObject barrelPrefab;

	public GameObject createBarrel () {
		return (GameObject) Instantiate (barrelPrefab);
	}
}