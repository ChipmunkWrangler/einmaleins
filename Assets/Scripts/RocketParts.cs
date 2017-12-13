using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketParts : MonoBehaviour {
	// public readonly
	public static RocketParts instance { get; private set; }

	public bool justUpgraded {
		get {
			return data.justUpgraded;
		}
		set {
			data.justUpgraded = value;
			data.Save();
		}
	}

	public int numParts {
		get {
			return data.numParts;
		}
		private set {
			data.numParts = value;
			data.Save();
		}
	}

	public bool isRocketBuilt {
		get {
			return data.isRocketBuilt;
		}
		set {
			data.isRocketBuilt = value;
			data.Save();
		}
	}

	public int upgradeLevel {
		get {
			return data.upgradeLevel;
		}
		private set {
			data.upgradeLevel = value;
			data.Save();
		}
	}

	public int numPartsRequired { 
		get {
			return PARTS_PER_UPGRADE;
		}
	}

	public bool hasEnoughPartsToUpgrade {
		get {
			return numParts >= PARTS_PER_UPGRADE && upgradeLevel < maxUpgradeLevel;
		}
	}

	public int maxUpgradeLevel {
		get {
			UnityEngine.Assertions.Assert.AreEqual( Questions.GetNumQuestions() % PARTS_PER_UPGRADE, 0 );
			return 1 + Questions.GetNumQuestions() / PARTS_PER_UPGRADE; // +1 for final bonus upgrade
		}
	}

	// public commands
	public bool Upgrade () {
		if (hasEnoughPartsToUpgrade) {
			numParts -= PARTS_PER_UPGRADE;
			DoUpgrade();
			return true;
		}
		return false;
	}

	public void Inc () {
		++numParts;
	}

	public void UnlockFinalUpgrade () {
		numParts += numPartsRequired;
	}

	public void Reset () {
		Destroy( RocketParts.instance );	
	}

	// private
	const int PARTS_TO_BUILD_ROCKET = 0;
	const int PARTS_PER_UPGRADE = 11;

	RocketPartsPersistantData data;

	void Awake () {
		if (instance == null) {
//			DontDestroyOnLoad (gameObject);
			instance = this;
			data = new RocketPartsPersistantData();
			data.Load();
		} else if (instance != this) {
			Destroy( gameObject ); // there can be only one!
		}
	}

	void DoUpgrade () {
		data.justUpgraded = true;
		++data.upgradeLevel;
		data.Save();
	}
}

[System.Serializable]
public class RocketPartsPersistantData {
	const string prefsKey = "rocketParts";

	public bool justUpgraded;
	public int numParts;
	public bool isRocketBuilt;
	public int upgradeLevel;

	public void Save () {
		MDPrefs.SetBool( prefsKey + ":isBuilt", isRocketBuilt );
		MDPrefs.SetInt( prefsKey + ":upgradeLevel", upgradeLevel );
		MDPrefs.SetBool( prefsKey + ":justUpgraded", justUpgraded );
		MDPrefs.SetInt( prefsKey, numParts );
	}

	public void Load () {
		numParts = MDPrefs.GetInt( prefsKey, 0 );
		isRocketBuilt = MDPrefs.GetBool( prefsKey + ":isBuilt" );
		upgradeLevel = MDPrefs.GetInt( prefsKey + ":upgradeLevel", 0 );
		justUpgraded = MDPrefs.GetBool( prefsKey + ":justUpgraded" );
	}
}