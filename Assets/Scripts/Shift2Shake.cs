using EZCameraShake;
using UnityEngine;

public class Shift2Shake : MonoBehaviour
{
    public CameraShaker shaker;

    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            shaker.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
    }
}