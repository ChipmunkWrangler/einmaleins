using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlanet {
	const string prefsKey = "targetPlanet";

	static int targetPlanetIdx = -1;

	public static readonly float[] heights = {
		7.8e+07f,
		6.3e+08f,
		1.287e+09f,
		2.73e+09f,
		4.357e+09f,
		5.772e+09f
	};

	public static int RecordIfPlanetReached(float rocketHeight) {
		int oldIdx = -1;
		if (rocketHeight > GetTargetPlanetHeight ()) {
			oldIdx = GetIdx ();
			SetIdx (oldIdx + 1);
		}
		return oldIdx;
	}

	public static int GetIdx() {
		if (targetPlanetIdx < 0) {
			targetPlanetIdx = MDPrefs.GetInt (prefsKey, 0);
		}
		return targetPlanetIdx; 
	}

	public static int GetNumPlanets() {
		return heights.Length;
	}

	static float GetTargetPlanetHeight() {
		return (GetIdx() < heights.Length) ? heights [GetIdx ()] : float.MaxValue;
	}

	static void SetIdx(int newIdx ) {
		MDPrefs.SetInt (prefsKey, newIdx);
		targetPlanetIdx = newIdx;
	}

}
