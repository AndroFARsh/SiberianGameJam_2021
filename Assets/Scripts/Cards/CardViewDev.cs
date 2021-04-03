using System;
using UnityEngine;

[RequireComponent(typeof(ScaleButton))]
public class CardViewDev : MonoBehaviour
{
    public event Action<Card, CityPlace> OnAddPart; 

    [SerializeField] private Card card;
    [SerializeField] private CityPlace place;
    
    ScaleButton button;

    private void Awake()
    {
        button = GetComponent<ScaleButton>();        
        button.onClick.AddListener(OnClickListener);
    }
    
    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
    
    private void OnClickListener()
    {
        // TODO: user should pick a card and drag&drop to place to build 
        OnAddPart?.Invoke(card, place);
    }
}
