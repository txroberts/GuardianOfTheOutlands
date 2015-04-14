using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour {

	int waveCount;
	Text counterGUI;
	public Text endGameCounter;

	void Start () {
		counterGUI = GetComponent<Text> ();
		waveCount = 1;
	}

	void Update () {
		counterGUI.text = waveCount.ToString ();
		endGameCounter.text = waveCount.ToString ();
	}

	public void incrementWaveCounter () {
		waveCount++;
	}
}