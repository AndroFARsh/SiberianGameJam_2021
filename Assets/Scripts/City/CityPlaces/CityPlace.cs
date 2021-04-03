using UnityEngine;
using Woodman;

public class CityPlace : MonoBehaviour
{
    // indicate which side of the city (left/right) the tool place is located
    // used to calculate te city tilt
    [SerializeField] private bool onLeft ;
    [SerializeField] private ItemType allowedItemType;
   
    private Card activeCard;
    private GameObject item;
    
    public int TiltFactor => onLeft ? -1 : 1;
    public Card Card => activeCard;
    public bool IsEmpty => activeCard == null || item == null;

    public bool TryApplyCard(Card card)
    {
        switch (card.Action)
        {
            case ActionType.Build:
                if (!IsEmpty || card.Type != allowedItemType) return false;

                activeCard = card;
                item = Instantiate(card.PrefabItemView, transform);
                item.name = $"{name}_{card.PrefabItemView.name}";
                
                return true;
            case ActionType.Destroy:
                if (IsEmpty || card.Type != allowedItemType) return false;
                
                activeCard = null;
                Destroy(item);
                item = null;
                
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
}
