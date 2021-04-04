using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemDrop : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Color normal = Color.white;
    [SerializeField] private Color allow = Color.green;
    [SerializeField] private Color deny  = Color.red;
    [SerializeField] private float scaleSelected = 2f;
    [SerializeField] private float duration = 0.5f;
    
    private Image image;
    private bool isSelected;
    private Vector3 initialScale;

    public event Func<Card, bool> CheckCard; 
    public event Func<Card, bool> TryApplyCard;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = normal;
        initialScale = transform.localScale;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && TryApplyCard != null &&
            eventData.pointerDrag.TryGetComponent<CardView>(out var cardView) &&
            TryApplyCard.Invoke(cardView.Card))
        {
            cardView.OnDrop();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
        {
            image.DOColor(normal, duration).SetAutoKill(true);
            transform.DOScale(initialScale, duration).SetAutoKill(true);
            
            isSelected = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && eventData.pointerDrag != null && CheckCard != null &&
            eventData.pointerDrag.TryGetComponent<CardView>(out var cardView))
        {
            var color = CheckCard.Invoke(cardView.Card) ? allow : deny;
            image.DOColor(color, duration).SetAutoKill(true);
            transform.DOScale(scaleSelected, duration).SetAutoKill(true);
            
            isSelected = true;
        }
    }
}
