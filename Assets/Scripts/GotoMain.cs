using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoMain : MonoBehaviour {

	public void LoadMainScene() {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("main");
	}
}
