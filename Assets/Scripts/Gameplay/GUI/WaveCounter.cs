using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour {

	Text counterGUI;
	int waveCounter;

	void Start () {
		counterGUI = GetComponent<Text> ();
		waveCounter = 1;
	}

	void Update () {
		counterGUI.text = waveCounter.ToString ();
	}

	public void incrementWaveCounter () {
		waveCounter++;
	}
}