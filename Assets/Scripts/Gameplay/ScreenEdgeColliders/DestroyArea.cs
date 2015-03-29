using UnityEngine;

public class DestroyArea : MonoBehaviour {


	void Start () {

	}

	void OnTriggerExit2D (Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer); // Get the layer name

		if (layerName.Equals ("Bullet (Player)"))
			c.gameObject.SetActive (false);
	}
}