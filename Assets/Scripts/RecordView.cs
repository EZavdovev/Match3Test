using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// ������, ����������� �������� ������� ��������
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
    /// �����, ��� ���������� ������ �������� ������� ��������
    /// </summary>
    /// <param name="place">����� � �������</param>
    /// <param name="date">���� ��������� � �������</param>
    /// <param name="score">���������� ������������ �����</param>
    /// <param name="isNewRecord">�������� �� ��� ������ ��� ���������� ��������</param>
    public void SetData(int place, string date, string score, bool isNewRecord)
    {
        placeLabel.text = $"{place}";
        dateLabel.text = date;
        scoreLabel.text = score;
        currentRecordImage.enabled = isNewRecord;
    }
}
