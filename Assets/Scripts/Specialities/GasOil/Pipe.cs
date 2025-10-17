using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// труба
/// </summary>
public class Pipe : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] AvailableSide sides;

    public AvailableSide Sides => sides;

    RectTransform rectTransform;

    Vector2Int prefPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// зажатие и премещение объекта
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera ?? Camera.main, 
            out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    /// <summary>
    /// конец пермещения объекта
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 newPosition = SpecialityManager.Instance.GasOil.SetGridCoord(rectTransform.anchoredPosition, this);
        SpecialityManager.Instance.GasOil.DeleteFromGrid(prefPosition, newPosition, this);

        rectTransform.anchoredPosition = newPosition;
    }
    
    /// <summary>
    /// возвращает свободные стороны трубы
    /// </summary>
    /// <returns></returns>
    public List<AvailableSide> GetAvailableSides()
    {
        List<AvailableSide> sidesList = new();
        if (sides.HasFlag(AvailableSide.Left)) sidesList.Add(AvailableSide.Left);
        if (sides.HasFlag(AvailableSide.Right)) sidesList.Add(AvailableSide.Right);
        if (sides.HasFlag(AvailableSide.Bottom)) sidesList.Add(AvailableSide.Bottom);
        if (sides.HasFlag(AvailableSide.Top)) sidesList.Add(AvailableSide.Top);

        return sidesList;
    }

    /// <summary>
    /// сменить свободные стороны трубы
    /// </summary>
    void ChangeAvailableSides()
    {
        int intSides = (int)sides;
        intSides = (intSides << 1);
        if ((intSides & 0b10000) == 0b10000)
        {
            intSides &= ~0b10000;
            intSides |= 1;
        }

        sides = (AvailableSide)intSides;
    }

    /// <summary>
    /// вращение трубы
    /// </summary>
    public void Rotate()
    {
        ChangeAvailableSides();
        rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, rectTransform.eulerAngles.y, rectTransform.eulerAngles.z - 90);
    }

    /// <summary>
    /// надатие на трубу
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        prefPosition = new Vector2Int((int)rectTransform.anchoredPosition.x, (int)rectTransform.anchoredPosition.y);
    }
}
