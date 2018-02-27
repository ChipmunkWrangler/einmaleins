using UnityEngine;
using System.Collections;

public class EffortTrackerConfig
{
    public const int MinFrustration = -2;
    public const int MaxFrustration = 3;
    public const int MinQuizzesPerDay = 3;
    public const float MinTimePerDay = 5 * 60.0F;
    public const int FrustrationWrong = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
    public const int FrustrationGiveUp = 2;
    public const int FrustrationRight = -1;
    public const int FrustrationFast = -2;
    public const int NumAnswersPerQuiz = 7; // the bigger this is, the more new questions the kid will be confronted with at once
    public const int GauntletAskListLength = 55;
    public const int NumAnswersLeftWhenLaunchCodeAsked = 3;
}
