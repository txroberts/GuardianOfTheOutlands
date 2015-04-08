using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	Text timerGUI;
	public bool enabled;

	void Start () {
		timerGUI = GetComponent<Text> ();
		enabled = true;
	}

	void Update () {
		if (enabled) {
			float time = Time.timeSinceLevelLoad;
			float mins = Mathf.Floor (time / 60);
			float secs = Mathf.Floor (time % 60);
			float millisecs = Mathf.Floor ((time * 100) % 100);
			timerGUI.text = mins.ToString ("00") + ":" + secs.ToString ("00") + ":" + millisecs.ToString ("00");
		}
	}
}