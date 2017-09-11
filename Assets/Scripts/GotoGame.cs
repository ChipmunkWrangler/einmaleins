using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoGame : MonoBehaviour {

	public void LoadGameScene() {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (MDPrefs.GetString ("statsFromScene", "main"));
	}
}
