using System.Collections;
using UnityEngine;

/// <summary>
/// менеджер проигрыша
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
    /// открыть окно прогрыша
    /// </summary>
    public void OpenLooseWindow()
    {
        looseWindow.SetActive(true);
    }

    /// <summary>
    /// закрыть окно проигрыша
    /// </summary>
    public void CloseLooseWindow()
    {
        looseWindow.SetActive(false);
    }

}
