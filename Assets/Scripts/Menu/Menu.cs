using UnityEngine;

public class Menu : MonoBehaviour {

	Animator animator;
	CanvasGroup canvasGroup;

	void Awake () {
		animator = GetComponent<Animator> ();
		canvasGroup = GetComponent<CanvasGroup> ();

		RectTransform rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0); // reposition from off-screen to the centre of the screen
	}

	void Update () {
		// if the animator state is not 'Open', disable the canvas group
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Open"))
			canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
		else // if the animator state is 'Open', enable the canvas group
			canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
	}

	// change the animator state (trigger transitions)
	public bool isOpen {
		get { return animator.GetBool ("isOpen"); }
		set { animator.SetBool ("isOpen", value); }
	}
}