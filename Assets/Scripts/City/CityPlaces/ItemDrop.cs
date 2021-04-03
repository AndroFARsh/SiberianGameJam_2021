using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemDrop : MonoBehaviour, IDropHandler
{
    public event Func<Card, bool> TryApplyCard; 

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && TryApplyCard != null &&
            eventData.pointerDrag.TryGetComponent<CardView>(out var cardView) &&
            TryApplyCard.Invoke(cardView.Card))
        {
            Destroy(eventData.pointerDrag);
        }
    }
}
