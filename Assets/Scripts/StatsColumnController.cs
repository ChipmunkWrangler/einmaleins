using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Text[] cells;

	public void SetMasteryLevel(int row, int masteryLevel) {
		cells [row].text = masteryLevel.ToString ();	
	}
}
