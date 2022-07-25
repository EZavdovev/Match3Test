using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Кнопка выхода
/// </summary>
public class ExitButton : AbstractButtonAction
{
    protected override void OnButtonClick()
    {
        Application.Quit();
    }
}
