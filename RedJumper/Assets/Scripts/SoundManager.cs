using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioTheme, audioGameComplete;
    private AudioSource[] audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponents<AudioSource>();
        audio[0] = audioTheme;
        audio[1] = audioGameComplete;

        audio[0].Play();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
