using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoMain : MonoBehaviour {

	public void LoadMainScene() {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("main");
	}
}
