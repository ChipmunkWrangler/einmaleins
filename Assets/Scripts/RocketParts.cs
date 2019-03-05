using CrazyChipmunk;
using UnityEngine;
using UnityEngine.Assertions;

internal class RocketParts : MonoBehaviour
{
    private const int PartsToBuildRocket = 0;

    [SerializeField] private RocketPartsPersistentData data;
    [SerializeField] private Prefs prefs;
    private int numQuestionsToFullUpgrades => QuestionGenerator.GetNumQuestions(prefs);
    private int PartsPerUpgrade => numQuestionsToFullUpgrades / 5;

    public static RocketParts Instance { get; private set; }

    public bool JustUpgraded
    {
        get => data.JustUpgraded;
        set
        {
            data.JustUpgraded = value;
            data.Save();
        }
    }

    public int NumParts
    {
        get => data.NumParts;
        private set
        {
            data.NumParts = value;
            data.Save();
        }
    }

    public bool IsRocketBuilt
    {
        get => data.IsRocketBuilt;
        set
        {
            data.IsRocketBuilt = value;
            data.Save();
        }
    }

    public int UpgradeLevel
    {
        get => data.UpgradeLevel;
        private set
        {
            data.UpgradeLevel = value;
            data.Save();
        }
    }

    public int NumPartsRequired => PartsPerUpgrade;

    public bool HasEnoughPartsToUpgrade => NumParts >= PartsPerUpgrade && UpgradeLevel < MaxUpgradeLevel;

    public int MaxUpgradeLevel
    {
        get
        {
            Assert.AreEqual(numQuestionsToFullUpgrades % PartsPerUpgrade, 0);
            return 1 + numQuestionsToFullUpgrades / PartsPerUpgrade; // +1 for final bonus upgrade
        }
    }

    public static void Reset()
    {
        if (Instance != null) Destroy(Instance);
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

    private void Awake()
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

    private void DoUpgrade()
    {
        data.JustUpgraded = true;
        ++data.UpgradeLevel;
        data.Save();
    }
}