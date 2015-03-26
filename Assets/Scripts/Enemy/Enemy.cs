using UnityEngine;

public class Enemy : MonoBehaviour {

	public string currentState = "No Target";

	public void setCurrentState(string newState){
		currentState = newState;
	}
}