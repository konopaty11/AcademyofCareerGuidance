using UnityEngine;

/// <summary>
/// �� ���������� ��� �������� �����
/// </summary>
public class DontDestroyOnLoadScene : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
