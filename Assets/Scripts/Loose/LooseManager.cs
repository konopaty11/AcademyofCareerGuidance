using System.Collections;
using UnityEngine;

/// <summary>
/// �������� ���������
/// </summary>
public class LooseManager : MonoBehaviour
{
    [SerializeField] GameObject looseWindow;

    public static LooseManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ������� ���� ��������
    /// </summary>
    public void OpenLooseWindow()
    {
        looseWindow.SetActive(true);
    }

    /// <summary>
    /// ������� ���� ���������
    /// </summary>
    public void CloseLooseWindow()
    {
        looseWindow.SetActive(false);
    }

}
