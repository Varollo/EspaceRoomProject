using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    public bool PlayAtStart;
    public bool Loop;

    public AudioSource Source
    {
        get => _source;
        set
        {
            _source = value;
            _source.clip = Clip;
            _source.outputAudioMixerGroup = MixerGroup;
            _source.loop = Loop;

            if (PlayAtStart)
            {
                _source.Play();
            }
        }
    }

    private AudioSource _source;
}
