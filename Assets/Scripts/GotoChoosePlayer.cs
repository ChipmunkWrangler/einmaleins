using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoChoosePlayer : MonoBehaviour {

	public void LoadChoosePlayerScene() {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("choosePlayer");
	}
}
