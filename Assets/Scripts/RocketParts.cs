using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketParts : MonoBehaviour {
	// public readonly
	public static RocketParts Instance { get; private set; }

	public bool JustUpgraded {
		get {
			return Data.justUpgraded;
		}
		set {
			Data.justUpgraded = value;
			Data.Save();
		}
	}

	public int NumParts {
		get {
			return Data.numParts;
		}
		private set {
			Data.numParts = value;
			Data.Save();
		}
	}

	public bool IsRocketBuilt {
		get {
			return Data.isRocketBuilt;
		}
		set {
			Data.isRocketBuilt = value;
			Data.Save();
		}
	}

	public int UpgradeLevel {
		get {
			return Data.upgradeLevel;
		}
		private set {
			Data.upgradeLevel = value;
			Data.Save();
		}
	}

	public int NumPartsRequired { 
		get {
			return PartsPerUpgrade;
		}
	}

	public bool HasEnoughPartsToUpgrade {
		get {
			return NumParts >= PartsPerUpgrade && UpgradeLevel < MaxUpgradeLevel;
		}
	}

	public int MaxUpgradeLevel {
		get {
			UnityEngine.Assertions.Assert.AreEqual( Questions.GetNumQuestions() % PartsPerUpgrade, 0 );
			return 1 + Questions.GetNumQuestions() / PartsPerUpgrade; // +1 for final bonus upgrade
		}
	}

	// public commands
	public bool Upgrade () {
		if (HasEnoughPartsToUpgrade) {
			NumParts -= PartsPerUpgrade;
			DoUpgrade();
			return true;
		}
		return false;
	}

	public void Inc () {
		++NumParts;
	}

	public void UnlockFinalUpgrade () {
		NumParts += NumPartsRequired;
	}

	public static void Reset () {
		if (Instance != null) {
			Destroy( RocketParts.Instance );	
		}
	}

	// private
    const int PartsToBuildRocket = 0;
    const int PartsPerUpgrade = 11;

    RocketPartsPersistantData Data;

	void Awake () {
		if (Instance == null) {
//			DontDestroyOnLoad (gameObject);
			Instance = this;
			Data = new RocketPartsPersistantData();
			Data.Load();
		} else if (Instance != this) {
			Destroy( gameObject ); // there can be only one!
		}
	}

	void DoUpgrade () {
		Data.justUpgraded = true;
		++Data.upgradeLevel;
		Data.Save();
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