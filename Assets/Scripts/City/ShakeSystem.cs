using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeSystem : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private float coefStrenght = 0.1f;
    [SerializeField] private int vibrato = 1;

    float magnitude;

    public bool IsShaking { get; private set; }
        
    private Tweener shake;

    public void StartShake(float strenght)
    {
        magnitude = strenght * coefStrenght;

        StopShake();

        IsShaking = true;

        shake = transform.DOShakePosition(duration, magnitude, vibrato).SetLoops(-1);
    }

    public void StopShake()
    {
        IsShaking = false;

        shake.Kill();
    }
}
