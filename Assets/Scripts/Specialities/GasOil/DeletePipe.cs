using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �������� �����
/// </summary>
public class DeletePipe : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// �������� image �� ������
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        Destroy(eventData.pointerDrag);
    }
}
