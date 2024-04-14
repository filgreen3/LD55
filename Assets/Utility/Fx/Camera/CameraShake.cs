using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour, ISystem
{
    public float Amplitude = 1;
    public float Frequency = 1;
    public Transform _virtualCamera;

    private static CameraShake _instance;

    private void Start()
    {
        _instance = this;
    }

    public static void Shake(float amplitude, float frequency, float time)
    {
        _instance.Amplitude = amplitude;
        _instance.Frequency = frequency;
        _instance.StartCoroutine(_instance.ShakeCoroutine(time));

    }

    private IEnumerator ShakeCoroutine(float time)
    {
        var t = 0f;
        while (t < 1)
        {
            Amplitude = Mathf.Lerp(Amplitude, 0, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        Amplitude = 0;
    }

    private void Update()
    {
        _virtualCamera.localRotation = Quaternion.Euler(Vector3.forward * (Mathf.Sin(Time.time * Frequency) * Amplitude));
    }
}
