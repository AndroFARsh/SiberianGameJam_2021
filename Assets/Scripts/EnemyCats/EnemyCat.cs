using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyCat : MonoBehaviour
{
    public enum EnemyCatType {
        ENERGY_THIEF, 
        DESTROYER
    }

    private bool isEnemyForPlayer;
    private Vector3 HomePosition;
    public float duration = 5;

    public bool isAlive = false;

    public EnemyCatType type;

    public Sprite sprite;

    public EnemyCat(EnemyCatType type) {
        this.type = type;
        this.sprite = setSpiteByType(type);
        isAlive = true;
    }

    private Sprite setSpiteByType(EnemyCatType type)
    {
        switch(type) {
            case EnemyCatType.DESTROYER:
                // TODO return sprite for DESTROYER
                break;
            case EnemyCatType.ENERGY_THIEF:
                // TODO return sprite for ENERGY_THIEF
                break;
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        HomePosition = transform.position;
    }

    public void Attack(Transform target) 
    {
        if(isAlive) {
            transform.DOMove(target.position, duration);
        }
    }

    public void ReturnHome() 
    {
        if(isAlive) {
            transform.DOMove(HomePosition, duration);
        }
    }

}
