using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ����������� ���������� ������� � ����
/// </summary>
public class TryView : MonoBehaviour
{
    private TextMeshProUGUI tryText;

    private const string START_TEXT = "���������� �������: ";
    private void Awake()
    {
        tryText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        TryManager.OnValueChanged += ShowValue;
    }

    private void OnDisable()
    {
        TryManager.OnValueChanged -= ShowValue;
    }

    private void ShowValue(int score)
    {
        tryText.text = START_TEXT + score.ToString();
    }
}
