using UnityEngine;

public class EnemyCity : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    public void AttackTarget()
    {        
        int i = 0;
        
        int place = Random.Range(0, GameManager.PlayerCity.CityPlaces.Count);

        if (GameManager.PlayerCity.CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < GameManager.PlayerCity.CityPlaces.Count; i++)
            {
                if (!GameManager.PlayerCity.CityPlaces[i].IsEmpty)
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

        int place = Random.Range(0, GameManager.EnemyCity.CityPlaces.Count);
        Debug.Log(place);

        if (!GameManager.EnemyCity.CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < GameManager.EnemyCity.CityPlaces.Count; i++)
            {
                if (GameManager.EnemyCity.CityPlaces[i].IsEmpty)
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
        
        //GameManager.RequestCardOfType()
    }
}
