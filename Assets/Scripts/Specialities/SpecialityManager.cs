using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// менеджер специальностей
/// </summary>
public class SpecialityManager : MonoBehaviour
{
    [SerializeField] CreateLogo createLogo;
    [SerializeField] GasOil gasOil;
    [SerializeField] Saves saves;

    public GasOil GasOil => gasOil;
    public CreateLogo CreateLogo => createLogo;
    public Saves Saves => saves;

    public static SpecialityManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            return;

        Instance = this;
    }



}
