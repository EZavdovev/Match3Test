using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Контроллер числа попыток игрока
/// </summary>
public class TryManager : MonoBehaviour
{
    public static event Action<int> OnValueChanged = delegate { };
    public static event Action OnEndGame = delegate { };

    [SerializeField]
    private int startTry;

    [SerializeField]
    private int countBubbleForBonus;

    [SerializeField]
    private ScoreManager scoreManager;

    private int countTry;
    
    private void Awake()
    {
        countTry = startTry;
        OnValueChanged(countTry);
    }

    private void OnEnable()
    {
        GameFieldController.OnCountDestroyBubble += ChangeCount;
        GameFieldController.OnFieldEmpty += ScoreConvert;
    }

    private void OnDisable()
    {
        GameFieldController.OnCountDestroyBubble -= ChangeCount;
        GameFieldController.OnFieldEmpty -= ScoreConvert;
    }
    private void ChangeCount(int countBubble)
    {
        if(countBubble >= countBubbleForBonus)
        {
            countTry += (countBubble - countBubbleForBonus + 1);
        }
        else
        {
            countTry--;
        }
        OnValueChanged(countTry);
        if (countTry == 0)
        {
            OnEndGame();
        }
    }

    private void ScoreConvert()
    {
        scoreManager.AddScore(countTry);
        countTry = 0;
        OnValueChanged(countTry);
        OnEndGame();
    }

}
