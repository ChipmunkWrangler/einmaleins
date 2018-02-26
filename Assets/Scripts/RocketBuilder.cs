using UnityEngine;

class RocketBuilder : MonoBehaviour
{
    [SerializeField] float builtY = 0;
    [SerializeField] float hiddenY = -2.0F;
    [SerializeField] float maxY = 1.0F;
    [SerializeField] float buildingTime = 5.0F;
    [SerializeField] float upgradeFlightTime = 5.0F;
    [SerializeField] float buildingDelay = 1.0F;
    [SerializeField] RocketPartCounter counter = null;
    [SerializeField] UpgradeButton upgradeButton = null;
    [SerializeField] ParticleSystem buildParticles = null;
    [SerializeField] ParticleSystem[] exhaustParticles = null;
    [SerializeField] GameObject launchButton = null;
    [SerializeField] GameObject rocketPartsWidget = null;
    [SerializeField] GameObject chooseColourButton = null;

    iTween.EaseType[] easeTypes =
    {
        iTween.EaseType.linear,
        iTween.EaseType.easeInOutCubic,
        iTween.EaseType.easeOutQuad,
        iTween.EaseType.easeInOutQuint,
        iTween.EaseType.easeOutExpo,
        iTween.EaseType.linear,
        iTween.EaseType.easeOutExpo,
    };

    public void OnUpgrade()
    {
        UnityEngine.Assertions.Assert.AreEqual(exhaustParticles.Length - 1, RocketParts.Instance.MaxUpgradeLevel);
        if (RocketParts.Instance.Upgrade())
        {
            counter.Spend(RocketParts.Instance.NumParts + RocketParts.Instance.NumPartsRequired, RocketParts.Instance.NumParts);
            StartEngine();
            iTween.MoveTo(gameObject, iTween.Hash("y", maxY, "time", upgradeFlightTime, "delay", buildingDelay, "easetype", easeTypes[RocketParts.Instance.UpgradeLevel], "oncomplete", "Descend"));
        }
    }

    void Start()
    {
        chooseColourButton.SetActive(false);
        if (RocketParts.Instance.IsRocketBuilt)
        {
            SetY(builtY);
            rocketPartsWidget.SetActive(RocketParts.Instance.UpgradeLevel < RocketParts.Instance.MaxUpgradeLevel - 1);
            if (!RocketParts.Instance.HasEnoughPartsToUpgrade)
            {
                DoneBuildingOrUpgrading();
            }
        }
        else
        {
            SetY(hiddenY);
            rocketPartsWidget.SetActive(false);
            Build();
        }
    }

    void StartEngine()
    {
        for (int i = 0; i < exhaustParticles.Length; ++i)
        {
            if (exhaustParticles[i] != null)
            {
                exhaustParticles[i].gameObject.SetActive(false);
            }
        }
        int upgradeLevel = RocketParts.Instance.UpgradeLevel;
        exhaustParticles[upgradeLevel].gameObject.SetActive(true);
        exhaustParticles[upgradeLevel].Play();
    }

    void Descend()
    {
        GotoBasePos("DoneUpgrading");
    }

    void DoneUpgrading()
    {
        DoneBuildingOrUpgrading();
    }

    void GotoBasePos(string onComplete)
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", builtY, "time", buildingTime, "delay", buildingDelay, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", onComplete));
    }

    void Build()
    {
        GotoBasePos("DoneBuilding");
        buildParticles.Play();
        StartEngine();
    }

    void DoneBuilding()
    {
        RocketParts.Instance.IsRocketBuilt = true;
        buildParticles.Stop();
        DoneBuildingOrUpgrading();
    }

    void DoneBuildingOrUpgrading()
    {
        exhaustParticles[RocketParts.Instance.UpgradeLevel].Stop();
        upgradeButton.Hide();
        if (ChooseRocketColour.HasChosenColour())
        {
            launchButton.SetActive(true);
        }
        else
        {
            launchButton.SetActive(false);
            chooseColourButton.SetActive(true);
        }
    }

    void SetY(float y)
    {
        Vector3 pos = gameObject.transform.position;
        pos.y = y;
        gameObject.transform.position = pos;
    }
}
