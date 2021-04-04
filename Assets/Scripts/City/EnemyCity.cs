using UnityEngine;

public class EnemyCity : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    public void AttackTarget()
    {        
        int i = 0;
        var playerCity = GameManager.PlayerCity;
        int place = Random.Range(0, playerCity.CityPlaces.Count);

        if (playerCity.CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < playerCity.CityPlaces.Count; i++)
            {
                if (!playerCity.CityPlaces[i].IsEmpty)
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

        var card = GameManager.FindCard(playerCity.CityPlaces[place].ItemType, ActionType.Destroy);

        playerCity.CityPlaces[place].TryApplyCard(card);
    }

    public void BuildPlace()
    {
        int i = 0;

        var enemyCity = GameManager.EnemyCity;

        int place = Random.Range(0, enemyCity.CityPlaces.Count);

        if (!enemyCity.CityPlaces[place].IsEmpty)
        {
            for (i = 0; i < enemyCity.CityPlaces.Count; i++)
            {
                if (enemyCity.CityPlaces[i].IsEmpty)
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

        var card = GameManager.FindCard(enemyCity.CityPlaces[place].ItemType, ActionType.Build);
        enemyCity.CityPlaces[place].TryApplyCard(card);
    }
}
