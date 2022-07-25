using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Отображение количества попыток в игре
/// </summary>
public class TryView : MonoBehaviour
{
    private TextMeshProUGUI tryText;

    private const string START_TEXT = "Количество попыток: ";
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
