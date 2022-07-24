using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/// <summary>
/// ������, ������� ����������� �� ������ �����, �� � �����
/// </summary>
public class SceneChangeButton : ScreenChangeButton
{
    [SerializeField]
    private string nameScene;

    protected override void OnButtonClick()
    {
        SceneManager.LoadScene(nameScene);
        base.OnButtonClick();
    }
}
