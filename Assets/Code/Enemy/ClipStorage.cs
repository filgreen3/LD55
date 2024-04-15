using UnityEngine;

public class ClipStorage : MonoBehaviour, ISystem
{
    public static ClipStorage Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip _fire;
    public AudioClip _hit;
    public AudioClip _hit2;
    public AudioClip _lose;
    public AudioClip _win;
    public AudioClip _summon;
    public AudioClip _kill;
    public AudioClip _connect;
}