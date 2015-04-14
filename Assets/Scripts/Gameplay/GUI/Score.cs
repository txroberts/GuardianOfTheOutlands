using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	int score, playerDeaths;

	bool multiplierActive = false;
	public int scoreMultiplier;
	public float multiplierEffectTime;
	float multiplierEndTime;

	public Slider multiplierSlider;
	public Image multiplierSliderBackground, multiplierSliderFill;
	public Text multiplierGUI;

	Text scoreGUI;
	public Text endGameScore, endGamePlayerDeaths;

	Animator anim;

	void Start () {
		scoreGUI = GetComponent<Text> ();
		score = 0;
		playerDeaths = 0;

		scoreMultiplier = 1;
		multiplierSlider.maxValue = multiplierEffectTime;

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

	public void activateScoreMultiplier (Color powerUpColor) {
		scoreMultiplier += 1;
		multiplierGUI.color = powerUpColor;
		multiplierGUI.text = "x " + scoreMultiplier.ToString ();

		// make the timer slider the same colour as the power-up
		multiplierSliderBackground.color = new Color(powerUpColor.r, powerUpColor.g, powerUpColor.b, 0.5f);
		multiplierSliderFill.color = powerUpColor;

		multiplierSlider.value = multiplierEffectTime; // reset the time slider back to the effect length
		multiplierSlider.gameObject.SetActive (true);

		multiplierEndTime = Time.time + multiplierEffectTime; // calculate the time when the effect will expire
		multiplierActive = true;
	}
}