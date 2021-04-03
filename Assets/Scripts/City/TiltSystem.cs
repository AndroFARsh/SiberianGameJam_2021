using DG.Tweening;
using UnityEngine;


public class TiltSystem : MonoBehaviour
{
    [SerializeField] private float stepAngle = 15;
    [SerializeField] private float duration = 1;
    
    public void SetTilt(int tilt)
    {
        var angles = transform.localEulerAngles;
        transform.DORotate(new Vector3(angles.x, angles.y, tilt * stepAngle), duration);
    }
}
