using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour {

	public void LoadPreviousScene() {
		string sceneName = BackStack.Pop ();
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName);
	}
}
