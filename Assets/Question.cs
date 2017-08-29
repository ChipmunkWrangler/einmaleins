using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	private int a;
	private int b;
	private string[] answers;
	private float[] easinesses;
	private const float defaultEasiness = 2.5f;

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		answers = new string[] { (a * b).ToString () };
		easinesses = new float[answers.Length];
		for (int i = 0; i < easinesses.Length; ++i) {
			easinesses[i] = defaultEasiness;
		}
	}

	public string GetQuestionString() {
		int x = Random.Range(0,2);
		Debug.Log (x);
		return (x == 0) ? a + " x " + b : b + " x " + a;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		int correctAnswer;
		if (int.TryParse (answer, out result) && int.TryParse(answers[0], out correctAnswer)) {
			return result == correctAnswer;
		}
		return false;
	}

	public void Load(string prefsKey) {
		float[] loaded = PlayerPrefsArray.GetFloatArray(prefsKey + ":easinesses");
		if (loaded != null) {
			easinesses = loaded;
		}
	}

	public void Save(string prefsKey) {
		PlayerPrefsArray.SetFloatArray(prefsKey + ":easinesses", easinesses);
	}

	public override string ToString() {
		string str = a + " * " + b + " (";
		for (int i = 0; i < easinesses.Length; ++i) {
			str += easinesses [i];
		}
		str += ")";
		return str;
	}
}
