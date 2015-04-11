using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	int score, playerDeaths;

	Text scoreGUI;
	public Text endGameScore, endGamePlayerDeaths;

	Animator anim;

	void Start () {
		scoreGUI = GetComponent<Text> ();
		score = 0;
		playerDeaths = 0;

		anim = GetComponent<Animator> ();
	}

	void Update () {
		scoreGUI.text = score.ToString ();
		endGameScore.text = score.ToString ();
		endGamePlayerDeaths.text = playerDeaths.ToString ();
	}

	public void addPoints (int points) {
		score += points;
		anim.SetTrigger ("AddPoints");
	}

	public void subtractPoints (int points) {
		score -= points;
		playerDeaths++;
		anim.SetTrigger ("SubtractPoints");
	}
}