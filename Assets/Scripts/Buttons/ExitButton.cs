using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  нопка выхода
/// </summary>
public class ExitButton : AbstractButtonAction
{
    protected override void OnButtonClick()
    {
        Application.Quit();
    }
}
