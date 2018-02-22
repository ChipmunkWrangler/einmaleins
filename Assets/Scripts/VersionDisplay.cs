using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionDisplay : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Text text = null;
	void Start () {
		text.text = MDVersion.GetCurrentVersion ();
	}
}
