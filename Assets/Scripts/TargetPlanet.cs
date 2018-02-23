static class TargetPlanet
{
    public static readonly float[] Heights =
    {
        7.8e+07F,
        6.3e+08F,
        1.287e+09F,
        2.73e+09F,
        4.357e+09F,
        5.772e+09F
    };

    const string TargetKey = "targetPlanet";
    const string LastReachedKey = "lastReachedPlanet";
    const float FinalHeight = 9999999999F;

    static int targetPlanetIdx = -1;
    static int lastReachedPlanetIdx = -2;

    public static void Reset()
    {
        targetPlanetIdx = -1;
        lastReachedPlanetIdx = -2;
    }

    public static int GetLastReachedIdx()
    {
        if (lastReachedPlanetIdx < -1)
        {
            lastReachedPlanetIdx = MDPrefs.GetInt(LastReachedKey, -1);
        }
        return lastReachedPlanetIdx;
    }

    public static void SetLastReachedIdx(int planetIdx)
    {
        MDPrefs.SetInt(LastReachedKey, planetIdx);
        lastReachedPlanetIdx = planetIdx;
    }

    public static void TargetNextPlanet()
    {
        SetTargetPlanetIdx(GetTargetPlanetIdx() + 1);
    }

    public static int GetTargetPlanetIdx()
    {
        if (targetPlanetIdx < 0)
        {
            targetPlanetIdx = MDPrefs.GetInt(TargetKey, 0);
        }
        return targetPlanetIdx;
    }

    public static int GetMaxPlanetIdx() => Heights.Length - 1;
    public static float GetPlanetHeight(int i) => (i < Heights.Length) ? Heights[i] : FinalHeight;

    public static void SetTargetPlanetIdx(int newIdx)
    {
        MDPrefs.SetInt(TargetKey, newIdx);
        targetPlanetIdx = newIdx;
    }
}
