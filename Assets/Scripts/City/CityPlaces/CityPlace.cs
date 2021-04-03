using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Woodman;


public class CityPlace : MonoBehaviour
{
    // indicate which side of the city (left/right) the tool place is located
    // used to calculate te city tilt
    [SerializeField] private bool onLeft ;
    [SerializeField] private ItemType allowedItemType;
    [SerializeField] private ItemDrop itemDrop;
   
    private Card activeCard;
    private GameObject item;
    
    public int TiltFactor => onLeft ? -1 : 1;
    public Card Card => activeCard;
    public bool IsEmpty => activeCard == null || item == null;

    public event Action RequestRefresh; 
    
    private void Awake()
    {
        if (itemDrop)
        {
            itemDrop.TryApplyCard += TryApplyCard;
        }
    }

    public bool TryApplyCard(Card card)
    {
        switch (card.Action)
        {
            case ActionType.Build:
                if (!IsEmpty || card.Type != allowedItemType) return false;

                activeCard = card;
                item = Instantiate(card.PrefabItemView, transform);
                item.name = $"{name}_{card.PrefabItemView.name}";
                
                RequestRefresh?.Invoke();
                
                return true;
            case ActionType.Destroy:
                if (IsEmpty || card.Type != allowedItemType) return false;
                
                activeCard = null;
                Destroy(item);
                item = null;
                
                RequestRefresh?.Invoke();
                
                return true;
            default:
                Debug.LogError($"Unsupported action type {card.Action}");
                return true;
        }
    }

    private void OnDrawGizmos()
    {
        var tr = transform;
        var position = tr.position;
        DebugDraw.DrawCircle(Vector3.zero, 0.5f, Color.magenta, tr);
        DebugDraw.DrawMarker(position, 1f, Color.magenta);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && 
            eventData.pointerDrag.TryGetComponent<CardView>(out var cardView) && 
            TryApplyCard(cardView.Card))
        {
            Destroy(eventData.pointerDrag);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
    }
}
