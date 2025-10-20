using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    public static SoundController current;

    [SerializeField]
    public AudioSource staticSound;
    [SerializeField]
    public AudioSource musicSound;
    [SerializeField]
    public AudioSource breathSound;
    [SerializeField]
    public AudioSource doorSound;
    [SerializeField]
    public AudioSource stepSound;

    private void Awake()
    {
        current = this;
    }



}
