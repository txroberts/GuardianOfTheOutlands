using UnityEngine;

public class Enemy : MonoBehaviour {

	public string currentState = "No Target";
	public int pointsValue = 200;

	public void setCurrentState(string newState){
		currentState = newState;
	}
}