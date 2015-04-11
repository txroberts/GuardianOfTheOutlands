using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour {

	int waveCounter;
	Text counterGUI;
	public Text endGameCounter;

	void Start () {
		counterGUI = GetComponent<Text> ();
		waveCounter = 1;
	}

	void Update () {
		counterGUI.text = waveCounter.ToString ();
		endGameCounter.text = waveCounter.ToString ();
	}

	public void incrementWaveCounter () {
		waveCounter++;
	}
}