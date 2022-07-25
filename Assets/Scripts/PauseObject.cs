using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Объект, который при появлений останавливает игровой процесс
/// </summary>
public class PauseObject : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
