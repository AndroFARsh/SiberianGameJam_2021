using System;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace UnderwaterCats
{
    public struct CardComponent
    {
        public Card value;
    }
    
    public struct CardViewComponent
    {
        public CardView value;
    }

    public struct OnEnterHoverEvent
    {
    }
    
    public struct Hovered
    {
    }
    
    public struct OnExitHoverEvent
    {
    }
    
    public struct OnEnterDragEvent
    {
    }
    
    public struct OnExitDragEvent
    {
    }
    
    public class CardView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private TextMeshPro title;
        [SerializeField] private TextMeshPro description;
        [SerializeField] private TextMeshPro parameters;

        private EcsEntity entity;
        public void Init(EcsEntity e, Card card)
        {
            entity = e;
            name = card.Title;
            
            icon.sprite = card.Image;
            title.text = card.Title;
            description.text = card.Description;
            parameters.text = card.Parameters;
        }

        private void OnMouseEnter()
        {
            entity.Replace(new OnEnterHoverEvent());
        }   
        
        private void OnMouseExit()
        {
            entity.Replace(new OnExitHoverEvent());
        }

        private void OnMouseDown()
        {
            entity.Replace(new OnEnterDragEvent());
        }
        
        private void OnMouseUp()
        {
            entity.Replace(new OnExitDragEvent());
        }
    }
}