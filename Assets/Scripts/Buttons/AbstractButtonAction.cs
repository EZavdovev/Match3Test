using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Абстрактный класс, для описания действия для кнопки
/// </summary>
[RequireComponent(typeof(Button))]
public abstract class AbstractButtonAction : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    /// <summary>
    /// Действие, которое выполняется при нажатии кнопки
    /// </summary>
    protected abstract void OnButtonClick();

}
