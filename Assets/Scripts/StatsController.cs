using UnityEngine;

internal class StatsController : MonoBehaviour
{
    [SerializeField] private StatsColumnController[] columns;
    [SerializeField] private StatsControllerPersistentData data;
    [SerializeField] private Questions questions;

    private void Start()
    {
        data.Load(columns.Length);
        foreach (var question in questions.QuestionArray)
        {
            var i = question.A - 1;
            var j = question.B - 1;
            data.SeenMastered[i][j] = columns[i].SetMasteryLevel(j, question, data.SeenMastered[i][j]);
            if (i != j) data.SeenMastered[j][i] = columns[j].SetMasteryLevel(i, question, data.SeenMastered[j][i]);
        }

        foreach (var column in columns) column.DoneSettingMasteryLevels();
        data.Save(columns.Length);
    }
}