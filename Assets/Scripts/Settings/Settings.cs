using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���������
/// </summary>
public class Settings : MonoBehaviour
{
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider soundVolume;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource soundAudioSource;

    void OnEnable()
    {
        Saves.SavesLoad += RestoreVolume;

    }

    void Start()
    {
        List<Button> buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
       
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(soundAudioSource.Play);
        }

    }

    /// <summary>
    /// �������� ���������
    /// </summary>
    void RestoreVolume()
    {
        soundVolume.value = SpecialityManager.Instance.Saves.SavesData.SoundVolume;
        musicVolume.value = SpecialityManager.Instance.Saves.SavesData.MusicVolume;
        SoundVolumeControl(SpecialityManager.Instance.Saves.SavesData.SoundVolume);
        MusicVolumeControl(SpecialityManager.Instance.Saves.SavesData.MusicVolume);
    }

    /// <summary>
    /// �������� ��������� �����
    /// </summary>
    /// <param name="volume"></param>
    public void SoundVolumeControl(float volume)
    {
        soundAudioSource.volume = volume;
        SpecialityManager.Instance.Saves.SavesData.SoundVolume = volume;
    }

    /// <summary>
    /// �������� ��������� ������
    /// </summary>
    /// <param name="volume"></param>
    public void MusicVolumeControl(float volume)
    {
        musicAudioSource.volume = volume;
        SpecialityManager.Instance.Saves.SavesData.MusicVolume = volume;
    }
}
