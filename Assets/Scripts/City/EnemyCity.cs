using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCity : City
{
    [HideInInspector]
    public City Target;

    [SerializeField] private int attackCooldown = 10;

    [SerializeField] private float inactionDuration = 10f;

    Coroutine activation;
    Coroutine attackTarget;

    private bool isActivated;

    private void Awake()
    {
        activation = StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        while(inactionDuration > 0)
        {
            inactionDuration--;
            yield return new WaitForSeconds(1f);
        }

        isActivated = true;

        activation = null;

        if(attackTarget != null) StopCoroutine(attackTarget);
        attackTarget = StartCoroutine(AttackTarget());
    }

    private IEnumerator AttackTarget()
    {
        if (!isActivated) yield break;

        var choice = 0;
        while (Target)
        {
            choice = Random.Range(0, 3);

            if (choice < 1)
            {
                AttackCityPlaces(ItemType.Balloon);
            }
            else if (choice < 2)
            {
                StealStat(ItemType.Generator);
            }
            else if (choice < 3)
            {
                AttackCityPlaces(ItemType.Engine);
            }
            else 
            {
                AttackCityPlaces(ItemType.Gun);
            }


            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void StealStat(ItemType type)
    {
        foreach (var place in Target.CityPlaces)
        {
            if (place.ItemType == type && !place.IsEmpty)
            {
                //Steal
            }
        }
    }

    private void AttackCityPlaces(ItemType type )
    {
        foreach (var place in Target.CityPlaces)
        {
            if (place.ItemType == type && !place.IsEmpty)
            {
                //Attack
            }
        }
    }
}
