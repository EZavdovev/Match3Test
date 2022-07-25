using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// менеджер для работы с игровым интерфейсом
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject currentScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentScreen.SetActive(true);
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Меняет текущий экран на следующий
    /// </summary>
    /// <param name="nextScreen">следующий экран</param>
    public void ChangeScreen(GameObject nextScreen)
    {
        currentScreen.SetActive(false);
        currentScreen = nextScreen;
        currentScreen.SetActive(true);
    }
}

