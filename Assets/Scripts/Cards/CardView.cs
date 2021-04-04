
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI prameters;

    [SerializeField] private Card card;

    public Card Card => card;
    public event Action<CardView> OnDropHandler;
    public event Action<CardView> OnBeginDragHandler;
    public event Action<CardView, Vector2> OnDragHandler;
    public event Action<CardView> OnEndDragHandler;
    public event Action<CardView> OnPointerExitHandler;
    public event Action<CardView> OnPointerEnterHandler;
    
    public CanvasGroup CanvasGroup  { get; private set; }
    public RectTransform RectTransform  { get; private set; }
    
    public Vector3 InitialScale { get; private set; }
    public Vector2 InitialAnchoredPosition { get; private set; }
    
    private void OnValidate()
    {
        Init(card);
    }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        RectTransform = GetComponent<RectTransform>();

        InitialScale = RectTransform.localScale;
        InitialAnchoredPosition = RectTransform.anchoredPosition;
        
        Init(card);
    }

    public void Init(Card c)
    {
        card = c;
        image.sprite = c != null ? c.Image : null;
        title.text = c != null ? c.Title : String.Empty;
        description.text = c != null ?  c.Description : String.Empty;
        prameters.text = c != null ? c.Parameters : String.Empty;
    }

    public void SetParent(Transform parent, bool worldPositionStays=false) => RectTransform.SetParent(parent, worldPositionStays);

    public void OnBeginDrag(PointerEventData eventData) => OnBeginDragHandler?.Invoke(this);

    public void OnDrag(PointerEventData eventData) => OnDragHandler?.Invoke(this, eventData.delta);

    public void OnEndDrag(PointerEventData eventData) => OnEndDragHandler?.Invoke(this);

    public void OnDrop() => OnDropHandler?.Invoke(this);
    
    public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterHandler?.Invoke(this);

    public void OnPointerExit(PointerEventData eventData) => OnPointerExitHandler?.Invoke(this);
    
}
