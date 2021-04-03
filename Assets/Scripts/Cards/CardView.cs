
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Card card;
    
    public Card Card => card;

    private void OnValidate()
    {
        SetCard(card);
    }

    private void Awake()
    {
        SetCard(card);
    }

    public void SetCard(Card c)
    {
        card = c; 
        title.text = c?.Title;
        description.text = c?.Description;
    }
}
