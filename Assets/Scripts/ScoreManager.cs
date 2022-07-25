using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  онтроллер получени€ очков
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static event Action<long> OnValueChanged = delegate { };

    [SerializeField]
    private int scoreOneBubble;
    [SerializeField]
    private int multiplyScore;

    private long  score;
    
    private void Awake()
    {
        score = 0;
        OnValueChanged(score);
    }

    private void OnEnable()
    {
        GameFieldController.OnCountDestroyBubble += AddScore;
    }

    private void OnDisable()
    {
        GameFieldController.OnCountDestroyBubble -= AddScore;
    }
    /// <summary>
    /// ƒобавл€ет количество очков, по определенной формуле
    /// </summary>
    /// <param name="countBubble">число, которое конвертируетс€ в очки</param>
    public void AddScore(int countBubble)
    {
        score += (long)Mathf.Pow((scoreOneBubble * countBubble), multiplyScore);
        OnValueChanged(score);
    }
}
