using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс, который описывает игровой шар
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Animator))]
public class Bubble : MonoBehaviour
{
    public static event Action<int, int, Sprite> OnBubbleClick = delegate { };

    private int xPos;
    private int yPos;
    private Image image;
    private Button button;
    private RectTransform bubbleRectTransform;
    private Animator animator;

    private Coroutine changerPosition;
    private Coroutine destroyerBubble;

    private const float TIME_TO_CHANGE = 0.1f;
    private const string IS_BOOMING_KEY = "IsBooming";
    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(BubbleClick);
        bubbleRectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// Данный метод инициализирует данные шара
    /// </summary>
    /// <param name="xPos">нахождение шара в массиве по координате x</param>
    /// <param name="yPos">нахождение шара в массиве по координате y</param>
    /// <param name="sprite">изображение самого шара</param>
    public void Init(int xPos, int yPos, Sprite sprite)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        image.sprite = sprite;
    }
    /// <summary>
    /// Данный метод меняет позицию шара в массиве
    /// </summary>
    /// <param name="xPos">нахождение шара в массиве по координате x</param>
    /// <param name="yPos">нахождение шара в массиве по координате y</param>
    public void ChangePos(int xPos, int yPos)
    {
        changerPosition = StartCoroutine(ChangerPos(this.xPos, this.yPos, xPos, yPos));
        this.xPos = xPos;
        this.yPos = yPos;
    }

    private IEnumerator ChangerPos(int startX, int startY, int endX, int endY)
    {
        Vector3 endPositon = transform.position - new Vector3((startX - endX) * bubbleRectTransform.rect.width, (startY - endY) * bubbleRectTransform.rect.height);
        float time = 0f;
        Vector3 startPosition = transform.position;
        yield return new WaitForSeconds(TIME_TO_CHANGE);
        while(time < TIME_TO_CHANGE)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPositon, time / TIME_TO_CHANGE);
            yield return null;
        }
    }
    
    /// <summary>
    /// Сравнение спрайта с другим шаром
    /// </summary>
    /// <param name="sprite">спрайт другого шара</param>
    /// <returns></returns>
    public bool SpriteCheck(Sprite sprite)
    {
        return sprite == image.sprite;
    }
    private void BubbleClick()
    {
        OnBubbleClick(xPos, yPos, image.sprite);
    }

    private void OnDisable()
    {
        if(changerPosition != null)
        {
            StopCoroutine(changerPosition);
        }
        if(destroyerBubble != null)
        {
            StopCoroutine(destroyerBubble);
        }
    }
    /// <summary>
    /// Метод, для уничтожения шара
    /// </summary>
    public void DestroyBubble()
    {
        destroyerBubble = StartCoroutine(DestroyerBubble());
    }

    IEnumerator DestroyerBubble()
    {
        animator.SetBool(IS_BOOMING_KEY, true);
        yield return new WaitForSeconds(TIME_TO_CHANGE);
        animator.SetBool(IS_BOOMING_KEY, false);
        PoolManager.PutGameObjectToPool(gameObject);
    }
}
