using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDisplay : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Text questionText;

	public void DisplayQuestion(string _text) {
		questionText.text = _text;
	}
}
