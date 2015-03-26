using UnityEngine;

public class Barrel : MonoBehaviour {

	public bool targeted = false, pickedUp = false;

	void OnTriggerEnter2D (Collider2D c){
		// Get the layer name
		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		// ignore if it wasn't an enemy that collided
		if (!layerName.Equals ("Enemy"))
			return;

		GameObject enemy = c.gameObject;
		Enemy enemyScript = enemy.GetComponent<Enemy> ();
		EnemyMovement enemyMovementScript = enemy.GetComponent<EnemyMovement> ();

		// Only pick up a barrel if not already carrying one (none of the enemy's children are barrels)
		if (enemy.transform.FindChild("Barrel(Clone)") == null && !pickedUp){
			// If this wasn't the enemy's intended barrel, free the intended barrel for other enemies
			GameObject targetBarrel = enemyMovementScript.getTargetBarrel();
			if (!targetBarrel.Equals(gameObject)){
				Barrel targetBarrelScript = targetBarrel.GetComponent<Barrel>();
				targetBarrelScript.setTargeted(false);
			}

			// 'pick up the barrel' (make it a child of the enemy that collided with it)
			transform.parent = enemy.transform;
			targeted = false;
			pickedUp = true;

			enemyScript.setCurrentState("Escape");
		}
	}

	public void setTargeted (bool newValue){
		targeted = newValue;
	}

	public bool getTargeted (){
		return targeted;
	}

	public void setPickedUp (bool newValue){
		pickedUp = newValue;
	}

	public bool getPickedUp(){
		return pickedUp;
	}
}