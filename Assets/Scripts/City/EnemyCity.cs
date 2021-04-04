using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCity : City
{
    [HideInInspector]
    public City Target;

    public void AttackTarget()
    {        
        int i = 0;
        
        int place = Random.Range(0, Target.CityPlaces.Count);

        if (Target.CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < Target.CityPlaces.Count; i++)
            {
                if (!Target.CityPlaces[i].IsEmpty)
                {
                    place = i;
                    break;
                }
            }
        }
        else
        {
            i = place;
        }

        if (place != i)
        {
            Debug.LogError("No occupied target place");
            return;
        }

        //attack
        //Target.CityPlaces[place].
    }

    public void BuildPlace()
    {
        int i = 0;

        int place = Random.Range(0, CityPlaces.Count);
        Debug.Log(place);

        if (!CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < CityPlaces.Count; i++)
            {
                if (CityPlaces[i].IsEmpty)
                {
                    place = i;
                    break;
                }
            }
        }
        else
        {
            i = place;
        }

        if (place != i)
        {
            Debug.LogError("No empty target place");
            return;
        }

        //build
        //CityPlaces[place].
    }
}
