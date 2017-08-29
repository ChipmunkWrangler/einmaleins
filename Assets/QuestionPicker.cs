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

public class QuestionPicker : MonoBehaviour {
	private Question[] questions;
	const string prefsKey = "questions";
	[SerializeField] private int maxNum = 3;

	// Use this for initialization
	void Start () {
		InitEasiness ();
	}

	void InitEasiness() {		
		CreateQuestions ();
		Load ();
		for (int i = 0; i < questions.Length; ++i) {
			Debug.Log (questions [i]);
		}
		Save ();
	}

	void CreateQuestions() {
		questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = 1; b <= a; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
	}
	
	void Update () {
		
	}

	public void Reset() {
		PlayerPrefs.DeleteKey (prefsKey);
	}

	void Load() {
		if (PlayerPrefs.HasKey(prefsKey)) {
			UnityEngine.Assertions.Assert.AreEqual (PlayerPrefs.GetInt (prefsKey + ":ArrayLen"), questions.Length);
		}
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i.ToString ());
		}
	}

	void Save() {
		PlayerPrefs.SetInt(prefsKey + ":ArrayLen", questions.Length);
		for(int i = 0; i < questions.Length; ++i) {
			questions [i].Save (prefsKey + ":" + i.ToString());
		}
	}


}
