using UnityEngine;
using DG.Tweening;
using System;

public enum EnemyCatType {
    ENERGY_THIEF, 
    DESTROYER
}

public class EnemyCat : MonoBehaviour
{
    public EnemyCatType Type;
    public SpriteRenderer Sprite;

    public float Duration = 5;
    public bool IsAlive = false;

    private Vector3 homePosition;

    public Sprite DestroyerSprite;
    public Sprite ThiefSprite;

    public void CreateEnemyCat(EnemyCatType type) 
    {
        this.Type = type;
        Sprite.sprite = SetSpiteByType(type);
        IsAlive = true;
    }

    private Sprite SetSpiteByType(EnemyCatType type)
    {
        switch(type) {
            case EnemyCatType.DESTROYER:
                return DestroyerSprite;
                
            case EnemyCatType.ENERGY_THIEF:
                return ThiefSprite;
                
        }
        return null;
    }

    private void Start()
    {
        homePosition = transform.position;
    }

    public void Attack(Transform target, Action onComplete = null) 
    {
        if(IsAlive) 
        {
            transform.DOMove(target.position, Duration).OnComplete(() => 
            {
                onComplete?.Invoke(); 
            });
        }
    }

    public void ReturnHome(Action onComplete = null) 
    {
        if(IsAlive) 
        {
            transform.DOMove(homePosition, Duration).OnComplete(() =>
            {
                Destroy(gameObject);
                onComplete?.Invoke();
            }); 
        }
    }

}
