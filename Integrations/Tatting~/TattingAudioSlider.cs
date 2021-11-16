using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tatting;

public class TattingAudioSlider : TattingSlider.Behaviour
{
    AudioManager.AudioChannel channel;

    private void Start()
    {
        slider.value = Mathf.RoundToInt(AudioManager.GetVolume(channel) * (slider.displayStrings.Length - 1));
    }

    public override void OnValueChange(int max, int value)
    {
        AudioManager.SetVolume(channel, (float)max / (float)value);
    }
}
