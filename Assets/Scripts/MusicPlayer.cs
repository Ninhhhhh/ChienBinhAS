using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSouce, loopSouce;
    // Start is called before the first frame update
    void Start()
    {
        introSouce.Play();
        loopSouce.PlayScheduled(AudioSettings.dspTime + introSouce.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
