using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoStats : MonoBehaviour {

	public void LoadStatsScene(string fromScene) {
		MDPrefs.SetString ("statsFromScene", fromScene);
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("stats");
	}
}
