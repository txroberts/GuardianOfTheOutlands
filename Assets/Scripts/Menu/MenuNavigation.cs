using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {
	
	public void switchToScene (string newScene) {
		Application.LoadLevel (newScene);
	}
}