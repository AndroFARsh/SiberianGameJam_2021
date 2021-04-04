using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class ShakeSystem : MonoBehaviour
{
    [System.Serializable]
    public class TiltToShakeStrenth
    {
        [SerializeField] internal int vibrato = 1;
        [SerializeField] internal float magnitude = 0.1f;
        [SerializeField] internal int tilt = 0;
    }
    
    [SerializeField] private float duration = float.MaxValue;
    [SerializeField] private TiltToShakeStrenth[] tiltToShake; 
    
    private TiltToShakeStrenth currentConfig;
    private Tweener shake;
    
    private void OnValidate()
    {
        Array.Sort(tiltToShake, (v1, v2) => v1.tilt - v2.tilt);
    }

    private TiltToShakeStrenth Find(int tilt)
    {
        var mapper = Array.Find(tiltToShake, v1 => v1.tilt == tilt);
        if (mapper == null && tiltToShake.Length > 0)
        {
            mapper = tiltToShake[0];
        }

        return mapper;
    }

    public void OnStatRefreshed(City city, CityStats stats) 
    {
        var config = stats.Speed > 0 ? Find(Mathf.Abs(stats.Tilt)) : null;
        if (currentConfig != config)
        {
            currentConfig = config;
            
            shake?.Kill();

            if (config != null)
            {
                shake = transform.DOShakePosition(duration, config.magnitude, config.vibrato).SetLoops(-1);
            }
        }
    }
}
