using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public Menu currentMenu;

	void Start () {
		// switch to the default menu
		switchToMenu (currentMenu);
	}

	public void switchToMenu (Menu newMenu) {
		if (currentMenu != null) // a menu is currently active
			currentMenu.isOpen = false; // close it

		// open the new menu
		currentMenu = newMenu;
		currentMenu.isOpen = true;
	}

	public void switchToScene (string newScene) {
		StartCoroutine(loadScene(newScene));
	}

	IEnumerator loadScene (string newScene) {
		yield return new WaitForSeconds (3f);
		Application.LoadLevel (newScene);
	}
}