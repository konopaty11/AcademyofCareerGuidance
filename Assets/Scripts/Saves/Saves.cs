using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// класс данных для сохранения
/// </summary>
[System.Serializable]
public class SaveData
{
    public bool IsCreateLogoComplite = false;
    public bool IsColoringComplite = false;
    public bool IsCompareLogoComplite = false;
    public bool IsGuessBrandComplite = false;
    public bool IsIdentikitComplite = false;
    public bool IsPasswordComplite = false;
    public bool IsPipelineComplite = false;
    public float MusicVolume = 0f;
    public float SoundVolume = 0f;
}

/// <summary>
/// класс сохранения
/// </summary>
public class Saves : MonoBehaviour
{
    string path;

    public SaveData SavesData { get; set; }

    public static UnityAction SavesLoad;

    void Start()
    {
        try
        {
            string fileName = "UserData.json";
            path = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(path))
            {
                SavesData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path))
                    ?? new();
                Save();
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                File.Create(path).Dispose();
                SavesData = new SaveData();
                Save();

            }

            SavesLoad?.Invoke();

        }
        catch { }

    }

    /// <summary>
    /// сохранение данных
    /// </summary>
    public void Save()
    {
        if (SavesData != null)
        {
            File.WriteAllText(path, JsonUtility.ToJson(SavesData));

        }
    }

    void OnApplicationQuit()
    {
        Save();
    }

}
