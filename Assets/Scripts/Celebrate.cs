using System.Collections.Generic;
using UnityEngine;

internal class Celebrate : MonoBehaviour, IOnWrongAnswer, IOnQuestionChanged, IOnQuizAborted
{
    public static readonly float Duration = 3.0F;
    [SerializeField] private bool continueAfterQuestions;
    private Coroutine coroutine;
    [SerializeField] private ParticleSystem[] exhaustParticles;
    [SerializeField] private ParticleSystem fastAnswerParticles;
    private bool isCelebrating;
    [SerializeField] private ParticleSystem masteryParticles;
    [SerializeField] private QuestionPicker questionPicker;
    [SerializeField] private ParticleSystem smokeParticles;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        StopTimer();
        if (continueAfterQuestions && question == null)
            StartCelebrating(false, false); // indefinitely
        else
            StopCelebrating();
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        StopTimer();
        StopCelebrating();
        StopSmoke();
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        StopTimer();
        StopCelebrating();
    }

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        StopTimer();
        if (question == null || !question.IsLaunchCode)
        {
            var percentOn = question == null
                ? 1F
                : Mathf.Min(1F, FlashThrust.GetThrustFactor(question.GetLastAnswerTime()));
            coroutine = StartCoroutine(DoCelebration(
                question != null && question.GetLastAnswerTime() <= Question.FastTime, isNewlyMastered, percentOn));
        }
    }

    private IEnumerator<WaitForSeconds> DoCelebration(bool isFastAnswer, bool isNewlyMastered, float percentOn)
    {
        var exhaustTime = Duration * percentOn;
        if (exhaustTime > 0)
        {
            StartCelebrating(isFastAnswer, isNewlyMastered);
            yield return new WaitForSeconds(exhaustTime);
            StopCelebrating();
        }

        if (exhaustTime < Duration)
        {
            StartSmoke();
            yield return new WaitForSeconds(Duration - exhaustTime);
            StopSmoke();
        }

        questionPicker.NextQuestion();
    }

    private void StartCelebrating(bool isFastAnswer, bool isNewlyMastered)
    {
        if (!isCelebrating)
        {
            isCelebrating = true;
            GetExhaustParticles().gameObject.SetActive(true);
            GetExhaustParticles().Play();
            if (isNewlyMastered) masteryParticles.Play();
            if (isFastAnswer) fastAnswerParticles.Play();
        }
    }

    private void StopCelebrating()
    {
        if (isCelebrating)
        {
            GetExhaustParticles().Stop();
            masteryParticles.Stop();
            fastAnswerParticles.Stop();
            isCelebrating = false;
        }
    }

    private void StartSmoke()
    {
        smokeParticles.Play();
    }

    private void StopSmoke()
    {
        smokeParticles.Stop();
    }

    private void StopTimer()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private ParticleSystem GetExhaustParticles()
    {
        return exhaustParticles[RocketParts.Instance.UpgradeLevel];
    }
}