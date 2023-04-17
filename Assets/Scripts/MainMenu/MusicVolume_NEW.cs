using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MusicVolume_NEW : MonoBehaviour
{
    [Header("Components")] 
    public AudioMixerGroup _Mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private TMP_Text _textMusic;
    [SerializeField] private TMP_Text _textSounds;

    [Header("Keys")]
    [SerializeField] private string saveProcentVolumeMusic;
    [SerializeField] private string saveProcentVolumeSounds;
 
    
    private void Awake()
    {
        if (PlayerPrefs.HasKey(saveProcentVolumeMusic))
        {
            _musicSlider.value = PlayerPrefs.GetFloat(saveProcentVolumeMusic) / 100;
            _textMusic.SetText(PlayerPrefs.GetFloat(saveProcentVolumeMusic).ToString() + "%");

        }
        else
        {
            _musicSlider.value = 0.5f;
            PlayerPrefs.SetFloat(saveProcentVolumeMusic, Mathf.Round(_musicSlider.value * 100));
        }

        if (PlayerPrefs.HasKey(saveProcentVolumeSounds))
        {
            _effectsSlider.value = PlayerPrefs.GetFloat(saveProcentVolumeSounds) / 100;
            _textSounds.SetText(PlayerPrefs.GetFloat(saveProcentVolumeSounds)+ "%");
        }
        else
        {
            _effectsSlider.value = 0.5F;
            PlayerPrefs.SetFloat(saveProcentVolumeSounds,  Mathf.Round(_effectsSlider.value * 100));
        }
    }

    private void Start()
    {
        _musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume();});
        _effectsSlider.onValueChanged.AddListener(delegate { ChangeEffectsVolume();});
        _Mixer.audioMixer.SetFloat("Music", Mathf.Lerp(-60, 0, _musicSlider.value));
        _Mixer.audioMixer.SetFloat("Effects", Mathf.Lerp(-60, 0, _effectsSlider.value));
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat(saveProcentVolumeMusic, Mathf.Round(_musicSlider.value * 100));
        _textMusic.SetText(Mathf.Round(_musicSlider.value * 100).ToString() + "%");
        _Mixer.audioMixer.SetFloat("Music", Mathf.Lerp(-60, 0, _musicSlider.value));
        Message.showNotification("Музыка: "+Mathf.Round(_musicSlider.value * 100).ToString()+ "%");
    }

    public void ChangeEffectsVolume()
    {
        PlayerPrefs.SetFloat(saveProcentVolumeSounds, Mathf.Round(_effectsSlider.value * 100));
        _textSounds.SetText(Mathf.Round(_effectsSlider.value * 100).ToString() + "%");
        _Mixer.audioMixer.SetFloat("Effects", Mathf.Lerp(-60, 0, _effectsSlider.value));
        Message.showNotification("Звуковые эффекты: "+Mathf.Round(_effectsSlider.value * 100).ToString()+ "%");

    }
}
