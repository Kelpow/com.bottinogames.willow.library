using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tatting;


public class TattingAudioSlider : TattingSlider.Behaviour
{
    public AudioManager.AudioChannel channel;

    private void Start()
    {
        Debug.Log(AudioManager.GetVolume(channel));
        slider.SetValue(Mathf.RoundToInt(AudioManager.GetVolume(channel) * (slider.displayStrings.Length - 1)));
    }

    public override void OnValueChange(int max, int value)
    {
        AudioManager.SetVolume(channel, (float)value / (float)max);
    }
}
