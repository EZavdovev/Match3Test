using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Отображение количества очков в игре
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreView : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private const string START_TEXT = "Количество очков: ";
    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ScoreManager.OnValueChanged += ShowValue;
    }

    private void OnDisable()
    {
        ScoreManager.OnValueChanged -= ShowValue;
    }

    private void ShowValue(long score)
    {
        scoreText.text = START_TEXT + score.ToString();
    }
}
