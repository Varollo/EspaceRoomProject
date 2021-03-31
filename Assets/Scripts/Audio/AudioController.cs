using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _mix;
    [SerializeField] private Sound[] _sounds;

    public Sound GetSoundByName(string name) => System.Array.Find(_sounds, s => s.Name == name);

    private void Awake()
    {
        foreach (Sound sound in _sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Play(string soundName, float pitch = 1f, bool playFromStart = false)
    {
        Sound sound = GetSoundByName(soundName);

        if (sound == null) return;

        if (playFromStart)
        {
            sound.Source.Stop();
        }
        else if (sound.Source.isPlaying) return;

        sound.Source.pitch = pitch;
        sound.Source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sound = GetSoundByName(soundName);

        if (sound == null || !sound.Source.isPlaying) return;

        sound.Source.Stop();
    }
}
