using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Ami.BroAudio;
public class CineShake : MonoBehaviour
{
    public static CineShake cShake;
    public CinemachineFreeLook cFreeLook;
    private float shakeTimer;
    [SerializeField] SoundID collisionSound;
    private void Awake()
    {
        if (cShake == null)
        {
            cShake = this;
        }
    }

    private void OnEnable()
    {
        CameraShakeEvent.OnShake += Shake;
    }

    private void OnDisable()
    {
        CameraShakeEvent.OnShake -= Shake;
    }

    public void Shake(float intensity, float time)
    {
        BroAudio.Play(collisionSound);
        SetShakeIntensity(intensity);
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            SetShakeIntensity(0f);
        }
    }

    private void SetShakeIntensity(float intensity)
    {
        // Apply shake to all three rigs
        for (int i = 0; i < 3; i++)
        {
            CinemachineBasicMultiChannelPerlin noise = cFreeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.m_AmplitudeGain = intensity;
            }
        }
    }
}
