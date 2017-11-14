using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionDisplay : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Text text;
	void Start () {
		text.text = CCVersion.GetCurrentVersion ();
	}
}
