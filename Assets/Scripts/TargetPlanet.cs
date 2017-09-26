using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlanet {
	const string prefsKey = "targetPlanet";

	static int targetPlanetIdx = -1;

	static readonly string[] reachedTexts = {
		"Mars erreicht!",
		"Jupiter erreicht!",
		"Saturn erreicht!",
		"Uranus erreicht!",
		"Neptun erreicht!",
		"Ende erreicht!" // should never happen
	};
		
	public static readonly float[] heights = {
		7.8e+07f,
		6.3e+08f,
		1.287e+09f,
		2.73e+09f,
		4.357e+09f
	};

	public static string RecordIfPlanetReached(float rocketHeight) {
		string reachedText = "";
		if (rocketHeight > GetTargetPlanetHeight ()) {
			reachedText = GetReachedText ();
			SetIdx (GetIdx () + 1);
		}
		return reachedText;
	}

	public static int GetIdx() {
		if (targetPlanetIdx < 0) {
			targetPlanetIdx = MDPrefs.GetInt (prefsKey, 0);
		}
		return targetPlanetIdx; 
	}

	static string GetReachedText() {
		return reachedTexts [GetIdx ()];
	}

	static float GetTargetPlanetHeight() {
		return (GetIdx() < heights.Length) ? heights [GetIdx ()] : float.MaxValue;
	}

	static void SetIdx(int newIdx ) {
		MDPrefs.SetInt (prefsKey, newIdx);
		targetPlanetIdx = newIdx;
	}

}
