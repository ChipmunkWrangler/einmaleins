﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoBuildRocket : MonoBehaviour {

	public void LoadScene() {
		BackStack.PushCurrentScene();
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("rocketBuilding");
	}
}