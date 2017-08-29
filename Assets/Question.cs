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
		return a + " x " + b;
	}

	public void Load(string prefsKey) {
		//		PlayerPrefs.SetInt (prefsKey + ":a", a);
		//		PlayerPrefs.SetInt (prefsKey + ":b", b);
		//		PlayerPrefsArray.SetStringArray(prefsKey + ":answers", answers);
		float[] loaded = PlayerPrefsArray.GetFloatArray(prefsKey + ":easinesses");
		if (loaded != null) {
			easinesses = loaded;
		}
	}

	public void Save(string prefsKey) {
		//		PlayerPrefs.SetInt (prefsKey + ":a", a);
		//		PlayerPrefs.SetInt (prefsKey + ":b", b);
		//		PlayerPrefsArray.SetStringArray(prefsKey + ":answers", answers);
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
