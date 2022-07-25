using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �����, ������� ��������� ������� ���
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
    /// ������ ����� �������������� ������ ����
    /// </summary>
    /// <param name="xPos">���������� ���� � ������� �� ���������� x</param>
    /// <param name="yPos">���������� ���� � ������� �� ���������� y</param>
    /// <param name="sprite">����������� ������ ����</param>
    public void Init(int xPos, int yPos, Sprite sprite)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        image.sprite = sprite;
    }
    /// <summary>
    /// ������ ����� ������ ������� ���� � �������
    /// </summary>
    /// <param name="countWidthBubble">���������� ����� � ������ �� ����</param>
    /// <param name="countHeightBubble">���������� ����� � ������ �� ����</param>
    /// <param name="xPos">���������� ���� � ������� �� ���������� x</param>
    /// <param name="yPos">���������� ���� � ������� �� ���������� y</param>
    public void ChangePos(int countWidthBubble, int countHeightBubble, int xPos, int yPos)
    {
        changerPosition = StartCoroutine(ChangerPos(countWidthBubble, countHeightBubble, xPos, yPos));
        this.xPos = xPos;
        this.yPos = yPos;
    }

    private IEnumerator ChangerPos(int countWidthBubble, int countHeightBubble, int endX, int endY)
    {
        Vector3 endPositon = new Vector3((endX - countWidthBubble / 2) * bubbleRectTransform.rect.width + (1 - countWidthBubble % 2) * bubbleRectTransform.rect.width / 2, (endY - countHeightBubble / 2) * bubbleRectTransform.rect.height + (1 - countHeightBubble % 2) * bubbleRectTransform.rect.height / 2);

        float time = 0f;
        Vector3 startPosition = transform.localPosition;
        yield return new WaitForSeconds(TIME_TO_CHANGE);
        while(time < TIME_TO_CHANGE)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, endPositon, time / TIME_TO_CHANGE);
            yield return null;
        }
    }
    
    /// <summary>
    /// ��������� ������� � ������ �����
    /// </summary>
    /// <param name="sprite">������ ������� ����</param>
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
    /// �����, ��� ����������� ����
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
