using UnityEngine;

public class StatsController : MonoBehaviour
{
    readonly StatsControllerPersistentData data = new StatsControllerPersistentData();

    [SerializeField] StatsColumnController[] columns = null;
    [SerializeField] Questions questions = null;

    void Start()
    {
        data.Load(columns.Length);
        foreach (Question question in questions.QuestionArray)
        {
            int i = question.A - 1;
            int j = question.B - 1;
            data.SeenMastered[i][j] = columns[i].SetMasteryLevel(j, question, data.SeenMastered[i][j]);
            if (i != j)
            {
                data.SeenMastered[j][i] = columns[j].SetMasteryLevel(i, question, data.SeenMastered[j][i]);
            }
        }
        foreach (StatsColumnController column in columns)
        {
            column.DoneSettingMasteryLevels();
        }
        data.Save(columns.Length);
    }
}
