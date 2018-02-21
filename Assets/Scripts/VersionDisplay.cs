using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionDisplay : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Text Text = null;
	void Start () {
		Text.text = MDVersion.GetCurrentVersion ();
	}
}
