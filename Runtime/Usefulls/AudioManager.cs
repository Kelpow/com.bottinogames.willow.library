using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Willow.Library;

public class AudioManager : MonoBehaviour
{
    const string VOLUME_SAVE_STRING = "WillowAudioManager_";

    public enum AudioChannel
    {
        Master,
        SFX,
        Music,
        Dialogue,
        Voice,
    }

    public static AudioManager instance;
    public void Awake() {
        if (instance)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        foreach(AudioChannel channel in (AudioChannel[])Enum.GetValues(typeof(AudioChannel)))
        {
            string name = channel.ToString();
            SetVolume(name, PlayerPrefs.GetFloat(VOLUME_SAVE_STRING + name, 1f));
        }
    }


    public AudioMixer mixer;
    
    public static void SetVolume(AudioChannel channel, float volume)
    {
        SetVolume(channel.ToString(), volume);
    }

    private static void SetVolume(string channel, float volume)
    {
        if (instance)
        {
            if (instance.mixer)
            {
                if (instance.mixer.GetFloat(channel, out float v))
                {
                    PlayerPrefs.SetFloat(VOLUME_SAVE_STRING + channel, 1f);
                    instance.mixer.SetLinearVolume(channel, volume);
                }
                else
                    Debug.LogWarning($"The audio mixer does not have a channel named {channel}!", instance);
            }
            else
                Debug.LogWarning("No audio mixer has been assigned in the AudioManager inspector!", instance);
        }
        else
            Debug.LogWarning("No AudioManager object exists in the scene!");
    }

    
    public static float GetVolume(AudioChannel channel)
    {
        return GetVolume(channel.ToString());
    }

    private static float GetVolume(string channel)
    {
        return PlayerPrefs.GetFloat(VOLUME_SAVE_STRING + channel, 1f);
    }
}
