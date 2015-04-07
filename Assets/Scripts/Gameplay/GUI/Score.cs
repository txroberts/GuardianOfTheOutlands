using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	Text scoreGUI;
	int score;

	Animator anim;

	void Start () {
		scoreGUI = GetComponent<Text> ();
		score = 0;

		anim = GetComponent<Animator> ();
	}

	void Update () {
		scoreGUI.text = score.ToString ();
	}

	public void addPoints (int points) {
		score += points;
		anim.SetTrigger ("AddPoints");
	}

	public void subtractPoints (int points) {
		score -= points;
		anim.SetTrigger ("SubtractPoints");
	}
}