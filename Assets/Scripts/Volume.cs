using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField] float musicLvl;
    [SerializeField] float effectsLvl;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MainMusic"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MainMusic");
            masterMixer.SetFloat("Main", PlayerPrefs.GetFloat("MainMusic"));
        }
        else
        {
            musicVolumeSlider.value = 0;
            masterMixer.SetFloat("Main", 0);
        }

        if (PlayerPrefs.HasKey("Effects"))
        {
            effectsVolumeSlider.value = PlayerPrefs.GetFloat("Effects");
            masterMixer.SetFloat("EffectParameter", PlayerPrefs.GetFloat("Effects"));
        }
        else
        {
            effectsVolumeSlider.value = 0;
            masterMixer.SetFloat("EffectParameter", 0);
        }
    }
    public void SetMusicVolume()
    {
        musicLvl = musicVolumeSlider.value;
        masterMixer.SetFloat("Main", musicLvl);
        PlayerPrefs.SetFloat("MainMusic", musicLvl);
    }

    public void SetEffectsVolume()
    {
        effectsLvl = effectsVolumeSlider.value;
        masterMixer.SetFloat("EffectParameter", effectsLvl);
        PlayerPrefs.SetFloat("Effects", effectsLvl);
    }

}
