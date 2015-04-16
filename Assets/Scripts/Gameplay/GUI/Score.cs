using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	int score, playerDeaths;

	bool multiplierActive;
	int scoreMultiplier;
	float multiplierEndTime;

	public Slider multiplierSlider;
	public Text multiplierGUI;

	Text scoreGUI;
	public Text endGameScore, endGamePlayerDeaths;

	Animator anim;

	void Start () {
		score = 0;
		playerDeaths = 0;

		multiplierActive = false;
		scoreMultiplier = 1;
		multiplierSlider.gameObject.SetActive (false);

		scoreGUI = GetComponent<Text> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {
		scoreGUI.text = score.ToString ();
		endGameScore.text = score.ToString ();
		endGamePlayerDeaths.text = playerDeaths.ToString ();

		if (multiplierActive) {
			if (Time.time < multiplierEndTime) {
				multiplierSlider.value -= 1 * Time.deltaTime;
			}
			else {
				multiplierActive = false;
				multiplierSlider.gameObject.SetActive (false);
				scoreMultiplier = 1;
			}
		}
	}

	public void addPoints (int points) {
		score += (points * scoreMultiplier);
		anim.SetTrigger ("AddPoints");
	}

	public void subtractPoints (int points) {
		score -= points;
		playerDeaths++;
		anim.SetTrigger ("SubtractPoints");
	}

	public void activateScoreMultiplier (float multiplierEffectTime) {
		scoreMultiplier += 1;
		multiplierGUI.text = "x " + scoreMultiplier.ToString ();

		// reset the time slider back to the effect length
		multiplierSlider.maxValue = multiplierEffectTime;
		multiplierSlider.value = multiplierEffectTime;
		multiplierSlider.gameObject.SetActive (true);

		multiplierEndTime = Time.time + multiplierEffectTime; // calculate the time when the effect will expire
		multiplierActive = true;
	}
}