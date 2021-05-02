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
    
    public class CardView: Converter
    {
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private TextMeshPro title;
        [SerializeField] private TextMeshPro description;
        [SerializeField] private TextMeshPro parameters;

        protected override void OnConvert(EcsWorld world, EcsEntity entity)
        {
            entity.Replace(new CardViewComponent {value = this});
            entity.Replace(new Ref<Transform>(transform));
                    
            entity.Replace(new Position {value = transform.position});
            entity.Replace(new Rotation {value = transform.eulerAngles});
            entity.Replace(new Scale {value = transform.localScale});
        }
        
        public void Init(Card card)
        {
            name = card.Title;
            
            icon.sprite = card.Image;
            title.text = card.Title;
            description.text = card.Description;
            parameters.text = card.Parameters;
        }
    }
}