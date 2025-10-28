using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// игра раскраски изображения
/// </summary>
public class ColoringMiniGame : Speciality
{
    [SerializeField] CaptureScreen captureScreen;
    [SerializeField] RectTransform captureArea;
    [SerializeField] List<ClickArea> areas;
    [SerializeField] GameObject readyBtn;
    [SerializeField] GameObject gameWindow;

    [SerializeField] Image targetImage;       
    [SerializeField] Color currentColor;

    public Color CurrentColor => currentColor;

    void Start()
    {
    }

    /// <summary>
    /// поставить текущий цвет
    /// </summary>
    /// <param name="color"></param>
    public void SetCurrentColor(Color color)
    {
        currentColor = color;
    }

    /// <summary>
    /// загрузка настроек
    /// </summary>
    public void RestoreSettings()
    {
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsColoringComplite;
    }

    /// <summary>
    /// начало игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete) return;
        gameWindow.SetActive(true);
    }

    public void CloseGame()
    {
        CaptureImage();
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForEndOfFrame();
        gameWindow.SetActive(false);
    }

    public void CheckAreas()
    {
        foreach (ClickArea area in areas)
        {
            if (!area.IsColored) return;
        }

        IsComplete = true;
        SpecialityManager.Instance.Saves.SavesData.IsColoringComplite = IsComplete;
        SpecialityManager.Instance.Saves.Save();
        readyBtn.SetActive(true);
    }

    void CaptureImage()
    {
        Vector3[] corners = new Vector3[4];
        captureArea.GetWorldCorners(corners);

        Canvas canvas = captureArea.GetComponentInParent<Canvas>();
        Camera camera = canvas?.worldCamera ?? Camera.main;

        for (int i = 0; i < 4; i++)
        {
            corners[i] = RectTransformUtility.WorldToScreenPoint(camera, corners[i]);
        }

        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);

        int width = (int)(maxX - minX);
        int height = (int)(maxY - minY);
        int x = (int)minX;
        int y = (int)minY;

        x = Mathf.Clamp(x, 0, Screen.width - width);
        y = Mathf.Clamp(y, 0, Screen.height - height);
        width = Mathf.Min(width, Screen.width - x);
        height = Mathf.Min(height, Screen.height - y);

        captureScreen.CaptureAreaAsSprite(width, height, x, y);
    }
}
