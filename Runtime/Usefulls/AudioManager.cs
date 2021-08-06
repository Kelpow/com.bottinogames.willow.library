using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Willow.Library;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel
    {
        Master,
        SFX,
        Music,
        Dialogue,
        Voice,
        
    }

    public static AudioManager instance;
    public void Awake() { if (instance) { Destroy(this); } else { instance = this; DontDestroyOnLoad(this.gameObject); } }


    public AudioMixer mixer;
    
    public static void SetVolume(AudioChannel channel, float volume)
    {
        SetVolume(channel.ToString(), volume);
    }

    public static void SetVolume(string channel, float volume)
    {
        if (instance)
        {
            if (instance.mixer)
            {
                if (instance.mixer.GetFloat(channel, out float v))
                {
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
}
