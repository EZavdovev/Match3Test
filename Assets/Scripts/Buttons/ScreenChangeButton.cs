using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������ ��� ������������ ������
/// </summary>
public class ScreenChangeButton : AbstractButtonAction
{
    [SerializeField]
    protected GameObject nextScreen;

    protected override void OnButtonClick()
    {
        UIManager.Instance.ChangeScreen(nextScreen);
    }
}
