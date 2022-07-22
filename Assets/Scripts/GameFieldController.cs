using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Контроллер, который занимается обработкой игрового поля
/// </summary>
public class GameFieldController : MonoBehaviour
{
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

    // Для проверки, только той области с которой начинаются лопания шариков
    private int leftIndexForCheck;
    private int downIndexForCheck;


    private void Awake()
    {
        InitField();
    }

    private void OnEnable()
    {
        Bubble.OnBubbleClick += UpdateField;
    }

    private void OnDisable()
    {
        Bubble.OnBubbleClick -= UpdateField;
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
                bubble.Init(j, i, allSpritesBubble[Random.Range(0, allSpritesBubble.Count)]);
                fieldBubbles[i, j] = bubble;
            }
        }
    }

    private void UpdateField(int startX, int startY, Sprite spriteChecker)
    {
        leftIndexForCheck = countWidthBubble;
        downIndexForCheck = countHeightBubble;
        SearchCombinationBubbles(startX, startY, spriteChecker);
        DestroyFoundBubbles();
        UpdatePositionField();

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
}
