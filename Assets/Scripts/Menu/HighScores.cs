using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

	Text[] highScoreGuiTexts;
	int numberOfRows;
	int numberOfColumns;

	void Start () {
		highScoreGuiTexts = gameObject.GetComponentsInChildren<Text> ();

		numberOfColumns = 4;
		numberOfRows = highScoreGuiTexts.Length / numberOfColumns;

		displayHighScores ();
	}

	void displayHighScores () {
		if (!PlayerPrefs.HasKey ("HS_score_0"))
			initialiseHighScores ();

		for (int i = 0; i < numberOfRows * numberOfColumns; i += numberOfColumns) {
			highScoreGuiTexts [i].text = ((i / numberOfColumns) + 1).ToString ();
			highScoreGuiTexts [i + 1].text = PlayerPrefs.GetInt ("HS_score_" + (i / numberOfColumns)).ToString ();
			highScoreGuiTexts [i + 2].text = PlayerPrefs.GetInt ("HS_waves_" + (i / numberOfColumns)).ToString ();
			highScoreGuiTexts [i + 3].text = PlayerPrefs.GetString ("HS_gameTime_" + (i / numberOfColumns));
		}
	}

	void initialiseHighScores () {
		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetInt ("HS_score_" + i, 0);
			PlayerPrefs.SetInt ("HS_waves_" + i, 0);
			PlayerPrefs.SetString ("HS_gameTime_" + i, "--:--:--");
			PlayerPrefs.SetString ("HS_timestamp_" + i, "--/--/-- --:--:--");
		}
	}
}