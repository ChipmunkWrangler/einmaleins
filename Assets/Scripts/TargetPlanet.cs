using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlanet {
	const string targetKey = "targetPlanet";
	const string lastReachedKey = "lastReachedPlanet";

	static int targetPlanetIdx = -1;
	static int lastReachedPlanetIdx = -2;

	public static readonly float[] heights = {
		7.8e+07f,
		6.3e+08f,
		1.287e+09f,
		2.73e+09f,
		4.357e+09f,
		5.772e+09f
	};

	const float FINAL_HEIGHT = 9999999999f;

	public static void Reset() {
		targetPlanetIdx = -1;
		lastReachedPlanetIdx = -2;
	}

	public static int GetLastReachedIdx() {
		if (lastReachedPlanetIdx < -1) {
			lastReachedPlanetIdx = MDPrefs.GetInt (lastReachedKey, -1);
		}
		return lastReachedPlanetIdx; 
	}

	public static void SetLastReachedIdx(int planetIdx) {
		MDPrefs.SetInt (lastReachedKey, planetIdx);
		lastReachedPlanetIdx = planetIdx;
	}

	public static void TargetNextPlanet() {
		SetIdx (GetTargetPlanetIdx() + 1);
	}

	public static int GetTargetPlanetIdx() {
		if (targetPlanetIdx < 0) {
			targetPlanetIdx = MDPrefs.GetInt (targetKey, 0);
		}
		return targetPlanetIdx; 
	}

	public static int GetMaxPlanetIdx() {
		return heights.Length - 1;
	}

	public static float GetPlanetHeight(int i) {
		return (i < heights.Length) ? heights [i] : FINAL_HEIGHT;
	}

	static void SetIdx(int newIdx ) {
		MDPrefs.SetInt (targetKey, newIdx);
		targetPlanetIdx = newIdx;
	}

}

[System.Serializable]
public class TargetPlanetPersistentData {
	public int targetPlanetIdx;
	public int lastReachedPlanetIdx;

	public void Load() {
		targetPlanetIdx = TargetPlanet.GetTargetPlanetIdx ();
		lastReachedPlanetIdx = TargetPlanet.GetLastReachedIdx ();
	}
}