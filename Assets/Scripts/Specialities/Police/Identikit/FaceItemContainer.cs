using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������� �������� ����
/// </summary>
public class FaceItemContainer : MonoBehaviour
{
    [SerializeField] ChoseItem choseItem;
    [SerializeField] FaceItem item;
    [SerializeField] Identikit identikit;

    GameObject jackdaw;
    
    /// <summary>
    /// ������� �������
    /// </summary>
    public void Choose()
    {
        if (jackdaw == null)
            jackdaw = transform.GetChild(0).gameObject;
        if (choseItem.Jackdaw != null)
            choseItem.Jackdaw.SetActive(false);

        jackdaw.SetActive(true);
        choseItem.Jackdaw = jackdaw;

        choseItem.SetFaceItem(item, GetComponent<Image>().sprite);
    }
}
