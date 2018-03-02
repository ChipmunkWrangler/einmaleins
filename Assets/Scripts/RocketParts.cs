using UnityEngine;

class RocketParts : MonoBehaviour
{
    const int PartsToBuildRocket = 0;
    const int PartsPerUpgrade = 11;

    [SerializeField] RocketPartsPersistentData data = null;

    public static RocketParts Instance { get; private set; }
    public bool JustUpgraded
    {
        get
        {
            return data.JustUpgraded;
        }
        set
        {
            data.JustUpgraded = value;
            data.Save();
        }
    }

    public int NumParts
    {
        get
        {
            return data.NumParts;
        }
        private set
        {
            data.NumParts = value;
            data.Save();
        }
    }

    public bool IsRocketBuilt
    {
        get
        {
            return data.IsRocketBuilt;
        }
        set
        {
            data.IsRocketBuilt = value;
            data.Save();
        }
    }

    public int UpgradeLevel
    {
        get
        {
            return data.UpgradeLevel;
        }
        private set
        {
            data.UpgradeLevel = value;
            data.Save();
        }
    }

    public int NumPartsRequired
    {
        get
        {
            return PartsPerUpgrade;
        }
    }

    public bool HasEnoughPartsToUpgrade
    {
        get
        {
            return NumParts >= PartsPerUpgrade && UpgradeLevel < MaxUpgradeLevel;
        }
    }

    public int MaxUpgradeLevel
    {
        get
        {
            UnityEngine.Assertions.Assert.AreEqual(Questions.GetNumQuestions() % PartsPerUpgrade, 0);
            return 1 + (Questions.GetNumQuestions() / PartsPerUpgrade); // +1 for final bonus upgrade
        }
    }

    public static void Reset()
    {
        if (Instance != null)
        {
            Destroy(RocketParts.Instance);
        }
    }

    public bool Upgrade()
    {
        if (HasEnoughPartsToUpgrade)
        {
            NumParts -= PartsPerUpgrade;
            DoUpgrade();
            return true;
        }
        return false;
    }

    public void Inc()
    {
        ++NumParts;
    }

    public void UnlockFinalUpgrade()
    {
        NumParts += NumPartsRequired;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            data.Load();
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // there can be only one!
        }
    }

    void DoUpgrade()
    {
        data.JustUpgraded = true;
        ++data.UpgradeLevel;
        data.Save();
    }
}
