using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoStats : MonoBehaviour {

	public void LoadStatsScene() {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("stats");
	}
}
