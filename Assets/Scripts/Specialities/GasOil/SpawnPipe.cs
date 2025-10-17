using UnityEngine;

/// <summary>
/// спавн труб
/// </summary>
public class SpawnPipe : MonoBehaviour
{
    [SerializeField] GameObject pipePrefab;
    [SerializeField] RectTransform parentSpawn;

    /// <summary>
    /// спавн труб по нажатию кнопки
    /// </summary>
    public void Spawn()
    {
        GameObject pipe = Instantiate(pipePrefab, parentSpawn);
        SpecialityManager.Instance.GasOil.AddPipe(pipe);
    }


}
