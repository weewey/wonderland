using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private static Transform _chatBubble;
    private static float _defaultSecondsToDestroy = 6.0f;

    public static void Create(Transform chatPrefab,
        Transform parent,
        Vector3 localPosition,
        string text
    )
    {
        if (_chatBubble)
        {
            DestroyImmediate(_chatBubble.gameObject);
            _chatBubble = InstantiateChatBubble(chatPrefab, parent, localPosition, text);
        }
        else
        {
            _chatBubble = InstantiateChatBubble(chatPrefab, parent, localPosition, text);
        }

        Destroy(_chatBubble.gameObject, _defaultSecondsToDestroy);
    }

    private static Transform InstantiateChatBubble(Transform chatPrefab,
        Transform parent,
        Vector3 localPosition,
        string text)
    {
        Transform chatBubbleTransform = Instantiate(chatPrefab, parent.position, Quaternion.identity, parent);
        chatBubbleTransform.localPosition = localPosition;
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
        return chatBubbleTransform;
    }

    public enum IconType
    {
        Happy,
        Neutral,
        Angry,
    }

    [SerializeField] private Sprite happyIconSprite;
    [SerializeField] private Sprite neutralIconSprite;
    [SerializeField] private Sprite angryIconSprite;


    private SpriteRenderer _backgroundSpriteRenderer;
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        _backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        _textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void Setup(string text)
    {
        UpdateText(text);
        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(0.2f, 0.1f);
        _backgroundSpriteRenderer.size = textSize + padding;
        _backgroundSpriteRenderer.transform.localPosition = new Vector3(0f, 0f);
        AddStaticTextWriter(text);
    }

    public void UpdateText(string text)
    {
        _textMeshPro.SetText(text);
        _textMeshPro.ForceMeshUpdate();
    }

    public void AddStaticTextWriter(string text)
    {
        TextWriter.AddWriter_Static(_textMeshPro, text, .03f, true, true, () => { });
    }
}