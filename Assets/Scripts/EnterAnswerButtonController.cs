using UnityEngine;

public class EnterAnswerButtonController : MonoBehaviour, OnWrongAnswer, OnQuizAborted, OnQuestionChanged, OnGiveUp {
	[SerializeField] UnityEngine.UI.Button button = null;

	bool isHiding;
	bool isShowing;

    public const float transitionTime = 0.25F;

	public void OnQuizAborted() {
		Hide ();
	}

	public void OnCorrectAnswer () {
		Hide ();
	}

	public void OnWrongAnswer (bool wasNew) {
		button.interactable = false; // don't hide, just show the give up button on top
	}

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			Hide ();
		} else {
			Show (); // behind the give up button
			button.interactable = false;
		}
	}

	public void OnAnswerChanged(bool isAnswerEmpty) {
		button.interactable = !isAnswerEmpty;
	}

	public void OnGiveUp(Question question) {
		Hide ();
	}

	void Show() {
		if (!isShowing) {
			ScaleTo (Vector3.one);
			isShowing = true;
		} 
		isHiding = false;
		button.interactable = true;
	}

	void Hide() {
	 	if (!isHiding) {
			ScaleTo (Vector3.zero);
			isHiding = true;
		}
		isShowing = false;
		button.interactable = false;
	}

	void ScaleTo(Vector3 tgtScale) {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}
}
