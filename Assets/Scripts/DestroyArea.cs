using UnityEngine;

public class DestroyArea : MonoBehaviour {

	void OnTriggerExit2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // Get the layer name

		// Destory anything that isn't the player and return straight away
		if (!layerName.Equals ("Player"))
			Destroy (c.gameObject);

		// The player 'screen wraps'
		Transform player = c.transform;
		Vector3 playerPosition = Camera.main.WorldToViewportPoint (player.position);
		Vector3 newPosition = player.position;
		
		if (playerPosition.x > 1 || playerPosition.x < 0)
			newPosition.x = -newPosition.x;
		
		if (playerPosition.y > 1 || playerPosition.y < 0)
			newPosition.y = -newPosition.y;
		
		player.position = newPosition;

	}
}