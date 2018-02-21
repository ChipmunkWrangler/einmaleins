﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetPlanet
{
	const string targetKey = "targetPlanet";
	const string lastReachedKey = "lastReachedPlanet";

	static int targetPlanetIdx = -1;
	static int lastReachedPlanetIdx = -2;

	public static readonly float[] heights = {
		7.8e+07F,
		6.3e+08F,
		1.287e+09F,
		2.73e+09F,
		4.357e+09F,
		5.772e+09F
	};

	const float FINAL_HEIGHT = 9999999999F;

	public static void Reset ()
	{
		targetPlanetIdx = -1;
		lastReachedPlanetIdx = -2;
	}

	public static int GetLastReachedIdx ()
	{
		if (lastReachedPlanetIdx < -1) {
			lastReachedPlanetIdx = MDPrefs.GetInt (lastReachedKey, -1);
		}
		return lastReachedPlanetIdx; 
	}

	public static void SetLastReachedIdx (int planetIdx)
	{
		MDPrefs.SetInt (lastReachedKey, planetIdx);
		lastReachedPlanetIdx = planetIdx;
	}

	public static void TargetNextPlanet ()
	{
		SetTargetPlanetIdx (GetTargetPlanetIdx () + 1);
	}

	public static int GetTargetPlanetIdx ()
	{
		if (targetPlanetIdx < 0) {
			targetPlanetIdx = MDPrefs.GetInt (targetKey, 0);
		}
		return targetPlanetIdx; 
	}

	public static int GetMaxPlanetIdx () => heights.Length - 1;
	public static float GetPlanetHeight (int i)	=> (i < heights.Length) ? heights [i] : FINAL_HEIGHT;

	public static void SetTargetPlanetIdx (int newIdx)
	{
		MDPrefs.SetInt (targetKey, newIdx);
		targetPlanetIdx = newIdx;
	}

}

[System.Serializable]
public class TargetPlanetPersistentData
{
    public int TargetPlanetIdx;
    public int LastReachedPlanetIdx;

	public void Load ()
	{
		TargetPlanetIdx = TargetPlanet.GetTargetPlanetIdx ();
		LastReachedPlanetIdx = TargetPlanet.GetLastReachedIdx ();
	}

	public void Save ()
	{
		TargetPlanet.SetTargetPlanetIdx (TargetPlanetIdx);
		TargetPlanet.SetLastReachedIdx (LastReachedPlanetIdx);
	}
}