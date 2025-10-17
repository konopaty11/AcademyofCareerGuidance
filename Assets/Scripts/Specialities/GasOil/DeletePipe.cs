using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// удаление трубы
/// </summary>
public class DeletePipe : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// бросание image на объект
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        Destroy(eventData.pointerDrag);
    }
}
