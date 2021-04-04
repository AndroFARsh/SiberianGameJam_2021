using System;
using UnityEngine;
using Woodman;
using UnityEngine.VFX;


public class CityPlace : MonoBehaviour
{
    // indicate which side of the city (left/right) the tool place is located
    // used to calculate te city tilt
    [SerializeField] private bool onLeft ;
    [SerializeField] private ItemType allowedItemType;
    public ItemType ItemType { get => allowedItemType;  }
    [SerializeField] private ItemDrop itemDrop;
    
    [SerializeField] private VisualEffect setAnimation;

   
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
            itemDrop.CheckCard += CheckCard;
            itemDrop.TryApplyCard += TryApplyCard;
        }
    }

    public bool CheckCard(Card card)
    {
        switch (card.Action)
        {
            case ActionType.Build:
                return IsEmpty && card.Type == allowedItemType;
            case ActionType.Destroy:
                return !IsEmpty && card.Type == allowedItemType;
            default:
                Debug.LogError($"Unsupported action type {card.Action}");
                return false;
        }
    }

    public bool TryApplyCard(Card card)
    {
        if (!CheckCard(card)) return false;
        
        switch (card.Action)
        {
            case ActionType.Build:
                setAnimation.Play();
                activeCard = card;
                item = Instantiate(card.PrefabItemView, transform);
                item.name = $"{name}_{card.PrefabItemView.name}";
                
                RequestRefresh?.Invoke();
                
                return true;
            case ActionType.Destroy:
                activeCard = null;
                Destroy(item);
                item = null;
                
                RequestRefresh?.Invoke();
                
                return true;
            default:
                return false;
        }
    }

    private void OnDrawGizmos()
    {
        var tr = transform;
        var position = tr.position;
        DebugDraw.DrawCircle(Vector3.zero, 0.5f, Color.magenta, tr);
        DebugDraw.DrawMarker(position, 1f, Color.magenta);
    }
}
