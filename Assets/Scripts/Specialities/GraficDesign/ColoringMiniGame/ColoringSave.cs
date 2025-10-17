using UnityEngine;

public class ColoringSave : MonoBehaviour
{
    [SerializeField] ColoringMiniGame coloring;

    void OnEnable()
    {
        Saves.SavesLoad += coloring.RestoreSettings;
    }

    void OnDisable()
    {
        Saves.SavesLoad -= coloring.RestoreSettings;
    }
}
