using UnityEngine;

public class DestroyArea : MonoBehaviour {

	void OnTriggerExit2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // Get the layer name

		if (layerName.Equals ("Bullet (Player)")) {
			c.gameObject.SetActive (false);
		} else if (layerName.Equals ("Player")) {
			// The player 'screen wraps'
			Transform player = c.transform;
			Vector3 playerPosition = Camera.main.WorldToViewportPoint (player.position);
			Vector3 newPosition = player.position;
		
			if (playerPosition.x > 1 || playerPosition.x < 0)
				newPosition.x = -newPosition.x;
		
			if (playerPosition.y > 1 || playerPosition.y < 0)
				newPosition.y = -newPosition.y;
		
			player.position = newPosition;
		} else {
			// Destroy anything else
			Destroy (c.gameObject);
		}
	}
}