using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// фигура
/// </summary>
public class Figure : MonoBehaviour, IDragHandler
{
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// зажатие и перемещение фигуры
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 startPos = rect.position;

        rect.position = eventData.position;

        if (SpecialityManager.Instance.CreateLogo.IsIntersect(rect))
            rect.position = startPos;
    }

    /// <summary>
    /// верхнея точка
    /// </summary>
    /// <returns></returns>
    public float GetTopPoint()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float top = corners[0].y;
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y > top)
                top = corners[i].y;
        }
        return top;
    }

    /// <summary>
    /// нижняя точка
    /// </summary>
    /// <returns></returns>
    public float GetBottomPoint()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float bottom = corners[0].y;
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y < bottom)
                bottom = corners[i].y;
        }
        return bottom;
    }

    /// <summary>
    /// левая точка
    /// </summary>
    /// <returns></returns>
    public float GetLeftPoint()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float left = corners[0].x;
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].x < left)
                left = corners[i].x;
        }
        return left;
    }

    /// <summary>
    /// правая точка
    /// </summary>
    /// <returns></returns>
    public float GetRightPoint()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float right = corners[0].x;
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].x > right)
                right = corners[i].x;
        }
        return right;
    }
}
