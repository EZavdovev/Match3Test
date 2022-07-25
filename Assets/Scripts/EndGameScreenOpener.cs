using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Скрипт, который занимается открытием окон по окончанию игры
/// </summary>
public class EndGameScreenOpener : MonoBehaviour
{
    [SerializeField]
    private GameObject loseScreen;
    [SerializeField]
    private GameObject winScreen;
    private void OnEnable()
    {
        Save.Instance.OnSaveDataEnd += EndLogic;
    }

    private void OnDisable()
    {
        Save.Instance.OnSaveDataEnd -= EndLogic;
    }

    private void EndLogic()
    {
        UIManager.Instance.ChangeScreen(Save.Instance.SavedDatas.Count >= Save.Instance.CurrentRecordPos ? winScreen : loseScreen);  
    }
}
