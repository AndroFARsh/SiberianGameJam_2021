using DG.Tweening;
using UnityEngine;


public class TiltSystem : MonoBehaviour
{
    [SerializeField] private float stepAngle = 15;
    [SerializeField] private float duration = 1;
        
    public void OnStatRefreshed(City city, CityStats stats)
    {    
        var angles = transform.localEulerAngles;

        transform.DORotate(new Vector3(angles.x, angles.y, stats.Tilt * stepAngle), duration);
    }
}
