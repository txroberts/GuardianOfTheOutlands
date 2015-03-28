using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour {

	Text scoreGUI;
	int score;

	void Start () {
		scoreGUI = GetComponent<Text> ();
		score = 0;
	}

	void FixedUpdate () {
		scoreGUI.text = score.ToString ();
	}

	public void addPoints (int points) {
		score += points;
	}
}