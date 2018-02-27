using UnityEngine;

// Designer-modifiable constants that describe how the player's effort data affects the quiz
[CreateAssetMenu(fileName = "Config", menuName = "TimesTables/EffortTrackerConfig", order = 1)]
public class EffortTrackerConfig : ScriptableObject
{
    public int MinFrustration = -2;
    public int MaxFrustration = 3;
    public int MinQuizzesPerDay = 3;
    public float MinTimePerDay = 5 * 60.0F;
    public int FrustrationWrong = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
    public int FrustrationGiveUp = 2;
    public int FrustrationRight = -1;
    public int FrustrationFast = -2;
    public int NumAnswersPerQuiz = 7; // the bigger this is, the more new questions the kid will be confronted with at once
    public int GauntletAskListLength = 55;
    public int NumAnswersLeftWhenLaunchCodeAsked = 3;
}
