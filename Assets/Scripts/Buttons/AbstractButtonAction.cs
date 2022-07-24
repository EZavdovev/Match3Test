using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����������� �����, ��� �������� �������� ��� ������
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
    /// ��������, ������� ����������� ��� ������� ������
    /// </summary>
    protected abstract void OnButtonClick();

}
