using UnityEngine;

/// <summary>
/// ����� ����
/// </summary>
public class SpawnPipe : MonoBehaviour
{
    [SerializeField] GameObject pipePrefab;
    [SerializeField] RectTransform parentSpawn;

    /// <summary>
    /// ����� ���� �� ������� ������
    /// </summary>
    public void Spawn()
    {
        GameObject pipe = Instantiate(pipePrefab, parentSpawn);
        SpecialityManager.Instance.GasOil.AddPipe(pipe);
    }


}
