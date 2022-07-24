using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/// <summary>
/// Кнопка, которая переключает не только экран, но и сцену
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
