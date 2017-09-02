using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoChoosePlayer : MonoBehaviour {

	public void LoadChoosePlayerScene() {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("choosePlayer");
	}
}
