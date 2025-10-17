using UnityEngine;

/// <summary>
/// не разрушение при загрузке сцены
/// </summary>
public class DontDestroyOnLoadScene : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
