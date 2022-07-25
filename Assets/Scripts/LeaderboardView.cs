using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Скрипт отображения таблицы рекордов
/// </summary>
public class LeaderboardView : MonoBehaviour
{
    [SerializeField]
    private RecordView resultViewPrefab;

    [SerializeField]
    private Transform resultsKeeper;

    private int prefabsAmount;

    private void OnEnable()
    {
        prefabsAmount = Save.Instance.SavedDatas.Count;
        for (int i = 0; i < prefabsAmount; i++)
        {
            var tmp = Instantiate(resultViewPrefab, resultsKeeper);
            tmp.SetData(i + 1, Save.Instance.SavedDatas[i].Date, Save.Instance.SavedDatas[i].Score, i + 1 == Save.Instance.CurrentRecordPos);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < prefabsAmount; i++)
        {
            Destroy(resultsKeeper.GetChild(i).gameObject);
        }
    }
}


