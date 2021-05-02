using UnityEngine;

namespace UnderwaterCats
{
    public interface IDeckService
    {
        public bool DrawCard(string playerId, out Card card, out CardView view);
    }
    
    public class DeckService: MonoBehaviour, IDeckService
    {
        [SerializeField] private Card[] uniqueCards;
        [SerializeField] private GameObject cardViewPrefab;
        
        public bool DrawCard(string playerId, out Card card, out CardView view)
        {
            if (uniqueCards == null || uniqueCards.Length == 0)
            {
                
                Debug.LogError("Unique card set is empty");
                
                card = null;
                view = null;
                
                return false;
            }

            var index = Random.Range(0, uniqueCards.Length);
            
            card = uniqueCards[index];
            var go = Instantiate(cardViewPrefab);

            return go.TryGetComponent(out view);
        }
    }
}