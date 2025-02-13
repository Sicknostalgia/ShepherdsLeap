using System;
using UnityEngine;

public class CameraShakeEvent : MonoBehaviour  //can call anywhere in game
{
    public static event Action<float, float> OnShake;

    public static void TriggerShake(float intensity, float duration)
    {
        OnShake?.Invoke(intensity, duration);
    }
}
