using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Скрипт, отображения элемента таблицы рекордов
/// </summary>
public class RecordView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI placeLabel;

    [SerializeField]
    private TextMeshProUGUI dateLabel;

    [SerializeField]
    private TextMeshProUGUI scoreLabel;

    [SerializeField]
    private Image currentRecordImage;
    /// <summary>
    /// Метод, для заполнения данных элемента таблицы рекордов
    /// </summary>
    /// <param name="place">Место в таблице</param>
    /// <param name="date">Дата появления в таблице</param>
    /// <param name="score">количество заработанных очков</param>
    /// <param name="isNewRecord">является ли это только что полученным рекордом</param>
    public void SetData(int place, string date, string score, bool isNewRecord)
    {
        placeLabel.text = $"{place}";
        dateLabel.text = date;
        scoreLabel.text = score;
        currentRecordImage.enabled = isNewRecord;
    }
}
