using EZCameraShake;
using UnityEngine;

public class FloatingShake : MonoBehaviour
{
    public CameraShaker shaker;
        
    public float Magnitude = 2f;
    public float Roughness = 10f;
    
    private void Start()
    {
        shaker.StartShake(Magnitude, Roughness, 5f);
    }
}
