using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class AudioHelper : MonoBehaviour, ISystem
{
    private ObjectPool<AudioSource> _pool;

    private static AudioHelper _instance;

    private void Awake()
    {
        _instance = this;
        Initialize();
    }

    private void Initialize()
    {
        _pool = new ObjectPool<AudioSource>(
        createFunc: () =>
        {
            var audio = new GameObject("audioSource", typeof(AudioSource)).GetComponent<AudioSource>();
            audio.transform.SetParent(transform);
            audio.playOnAwake = false;
            audio.loop = false;
            //audio.spatialBlend = 0.89f;
            audio.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("Mixers/Main");
            return audio;
        },
        //actionOnGet: noise => noise.gameObject.SetActive(true),
        //actionOnRelease: noise => noise.gameObject.SetActive(false),
        collectionCheck: false,
        defaultCapacity: 2,
        maxSize: 100);
    }

    public static void PlayClip(AudioClip clip, float pitchSpread = 0, float customPitch = 1)
    {
        if (clip == null) return;
        var audio = _instance._pool.Get();
        audio.pitch = Random.Range(customPitch - pitchSpread, customPitch + pitchSpread);
        _instance.StartCoroutine(_instance.PlayClip(audio, clip));
    }

    private IEnumerator PlayClip(AudioSource audio, AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
        yield return new WaitForSeconds(clip.length);
        _pool.Release(audio);
    }



}