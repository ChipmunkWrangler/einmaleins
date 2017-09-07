using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickoffMain : MonoBehaviour {
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] float delay = 0.5f;

	void Start () {
		StartCoroutine (Kickoff ());
	}
	
	IEnumerator Kickoff () {
		yield return new WaitForSeconds (delay);
		questionPicker.NextQuestion ();
	}
}
