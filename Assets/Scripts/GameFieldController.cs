using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Контроллер, который занимается обработкой игрового поля
/// </summary>
public class GameFieldController : MonoBehaviour
{
    public static event Action<int> OnCountDestroyBubble = delegate { };
    public static event Action OnFieldEmpty = delegate { };

    [SerializeField]
    private List<Sprite> allSpritesBubble = new List<Sprite>();
    [SerializeField]
    private RectTransform Panel;
    [SerializeField]
    private Bubble Bubble;

    [SerializeField]
    private int countHeightBubble;
    [SerializeField]
    private int countWidthBubble;

    private Bubble[,] fieldBubbles;

    private List<Bubble> bubblesForDestroy = new List<Bubble>();

    private Coroutine changerFieldTimer;
    private bool isChanging;

    // Для проверки, только той области с которой начинаются лопания шариков
    private int leftIndexForCheck;
    private int downIndexForCheck;

    private const float TIME_TO_CHANGE = 0.2f;


    private void Awake()
    {
        InitField();
    }

    private void OnEnable()
    {
        Bubble.OnBubbleClick += UpdateField;
        isChanging = false;
    }

    private void OnDisable()
    {
        Bubble.OnBubbleClick -= UpdateField;
        if(changerFieldTimer != null)
        {
            StopCoroutine(changerFieldTimer);
        }
    }

    private void InitField()
    {
        fieldBubbles = new Bubble[countHeightBubble, countWidthBubble];
        if (Bubble.TryGetComponent<RectTransform>(out RectTransform rectBubble))
        {
            float coeffWidth = Panel.rect.width / (countWidthBubble * rectBubble.rect.width);
            float coeffHeight = Panel.rect.height / (countHeightBubble * rectBubble.rect.height);
            float coeffMin = Mathf.Min(coeffHeight, coeffWidth);
            rectBubble.sizeDelta = new Vector2(rectBubble.rect.width * coeffMin, rectBubble.rect.height * coeffMin);
        }
        else
        {
            Debug.LogError("Данный префаб не подходит для создания поля");
            return;
        }
        for (int i = 0; i < countHeightBubble; i++)
        {
            for (int j = 0; j < countWidthBubble; j++)
            {
                PoolManager.GetGameObjectFromPool(Bubble.gameObject, Panel.transform).TryGetComponent<Bubble>(out Bubble bubble);
                if (bubble == null)
                {
                    Debug.LogError("Данный префаб не подходит для создания поля");
                    return;
                }
                float xPos = (j - countWidthBubble / 2) * rectBubble.rect.width + (1 - countWidthBubble % 2) * rectBubble.rect.width / 2;
                float yPos = (i - countHeightBubble / 2) * rectBubble.rect.height + (1 - countHeightBubble % 2) * rectBubble.rect.height / 2;
                bubble.transform.position += new Vector3(xPos, yPos, 0f);
                bubble.Init(j, i, allSpritesBubble[UnityEngine.Random.Range(0, allSpritesBubble.Count)]);
                fieldBubbles[i, j] = bubble;
            }
        }
    }

    private void UpdateField(int startX, int startY, Sprite spriteChecker)
    {
        if (isChanging)
        {
            return;
        }
        isChanging = true;
        leftIndexForCheck = countWidthBubble;
        downIndexForCheck = countHeightBubble;
        SearchCombinationBubbles(startX, startY, spriteChecker);
        OnCountDestroyBubble(bubblesForDestroy.Count);
        DestroyFoundBubbles();
        UpdatePositionField();
        CheckFieldEmpty();
        changerFieldTimer = StartCoroutine(ChangeFieldTimer());
    }

    private IEnumerator ChangeFieldTimer()
    {
        float time = 0f;
        while (time < TIME_TO_CHANGE)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isChanging = false;
    }

    private void SearchCombinationBubbles(int startX, int startY, Sprite spriteChecker)
    {
        if (startX < 0 || startX >= countWidthBubble || startY < 0 || startY >= countHeightBubble)
        {
            return;
        }
        if (fieldBubbles[startY, startX] == null || !fieldBubbles[startY, startX].SpriteCheck(spriteChecker))
        {
            return;
        }
        bubblesForDestroy.Add(fieldBubbles[startY, startX]);
        fieldBubbles[startY, startX] = null;

        leftIndexForCheck = leftIndexForCheck > startX ? startX : leftIndexForCheck;
        downIndexForCheck = downIndexForCheck > startY ? startY : downIndexForCheck;
        
        SearchCombinationBubbles(startX - 1, startY, spriteChecker);
        SearchCombinationBubbles(startX + 1, startY, spriteChecker);
        SearchCombinationBubbles(startX, startY - 1, spriteChecker);
        SearchCombinationBubbles(startX, startY + 1, spriteChecker);
        
        return;
    }

    private void DestroyFoundBubbles()
    {
        for(int i = 0; i < bubblesForDestroy.Count; i++)
        {
            bubblesForDestroy[i].DestroyBubble();
        }
        bubblesForDestroy.Clear();
    }

    private void UpdatePositionField()
    {
        int XPosOffset = 0;
        for(int i = leftIndexForCheck; i < countWidthBubble; i++)
        {
            bool isFirstEmptyInArray = false; // Переменная для поиска первого вхождения пустых мест на поле
            bool isFirstNotEmptyInArray = false; // Переменная для поиска первого вхождения не пустых мест на поле
            bool isSecondNotEmptyInArray = false; // Переменная для поиска второго вхождения не пустых мест, для того чтобы их начать опускать на пустые места
            int yPosOffset = 0;
            for(int j = downIndexForCheck; j < countHeightBubble; j++)
            {
                if(isFirstEmptyInArray && !isSecondNotEmptyInArray) // Если нашли пустые места, но не нашли не пустые элементы сверху, то увеличиваем шаг смещения вниз
                {
                    yPosOffset++;
                }

                if(fieldBubbles[j,i] == null && !isFirstEmptyInArray)
                {
                    isFirstEmptyInArray = true;
                }

                if(fieldBubbles[j,i] == null && isFirstEmptyInArray && isSecondNotEmptyInArray)
                {
                    break;
                }

                if (fieldBubbles[j, i] != null)
                {
                    isFirstNotEmptyInArray = true;

                    if (isFirstEmptyInArray)
                    {
                        isSecondNotEmptyInArray = true;
                        fieldBubbles[j - yPosOffset, i - XPosOffset] = fieldBubbles[j, i];
                        fieldBubbles[j - yPosOffset, i - XPosOffset].ChangePos(i - XPosOffset, j - yPosOffset);
                        fieldBubbles[j, i] = null;
                    }
                    else
                    {
                        if (XPosOffset > 0)
                        {
                            fieldBubbles[j, i - XPosOffset] = fieldBubbles[j, i];
                            fieldBubbles[j, i - XPosOffset].ChangePos(i - XPosOffset, j);
                            fieldBubbles[j, i] = null;
                        }
                    }
                }
            }

            if (!isFirstNotEmptyInArray && downIndexForCheck == 0) // Если столбец пустой, то смещаем влево столбцы
            {
                XPosOffset++;
            }
        } 
    }

    private void CheckFieldEmpty()
    {
        if(fieldBubbles[0,0] == null)
        {
            OnFieldEmpty();
        }
    }
}
