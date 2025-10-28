using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// игра создание логотипа
/// </summary>
public class CreateLogo : Speciality
{
    [SerializeField] CaptureScreen captureScreen;
    [SerializeField] GameObject gameWindow;
    [SerializeField] List<GameObject> figurePrefabs;
    [SerializeField] List<RectTransform> targetTransfroms;

    List<RectTransform> currentTransforms = new();
    List<GameObject> figures = new();
    List<RectTransform> figureRects = new();

    public List<GameObject> Figures => figures;

    void OnEnable()
    {
        Saves.SavesLoad += RestoreSettings;
    }

    void OnDisable()
    {
        Saves.SavesLoad -= RestoreSettings;
    }

    /// <summary>
    /// загрузка настроек
    /// </summary>
    void RestoreSettings()
    {
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsCreateLogoComplite;
    }

    /// <summary>
    /// начало игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete) return;
        gameWindow.SetActive(true);
        CreateRandomFigures();
    }

    /// <summary>
    /// скриншот логотипа
    /// </summary>
    public void CaptureLogo()
    {
        IsComplete = true;
        SpecialityManager.Instance.Saves.SavesData.IsCreateLogoComplite = IsComplete;
        SpecialityManager.Instance.Saves.Save();
        captureScreen.CaptureFiguresAsSprite();
        StartCoroutine(DisableGameWindow());
    }

    /// <summary>
    /// скрытие игры
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableGameWindow()
    {
        yield return new WaitForEndOfFrame();
        gameWindow.SetActive(false);
    }

    /// <summary>
    /// создание фигур в рандомных позициях
    /// </summary>
    public void CreateRandomFigures()
    {
        foreach (RectTransform rect in targetTransfroms)
            currentTransforms.Add(rect);

        if (figures.Count == 0)
            foreach (GameObject figurePrefab in figurePrefabs)
            {
                RectTransform rect = currentTransforms[Random.Range(0, currentTransforms.Count)];
                GameObject figure = Instantiate(figurePrefab, rect);

                figureRects.Add(figure.GetComponent<RectTransform>());
                figures.Add(figure);

                currentTransforms.Remove(rect);
            }
        else
            foreach (GameObject figure in figures)
            {
                RectTransform rect = currentTransforms[Random.Range(0, currentTransforms.Count)];
                RectTransform figureRect = figure.GetComponent<RectTransform>();

                figureRect.position = rect.position;

                currentTransforms.Remove(rect);
            }
    }

    /// <summary>
    /// проверка пересечания фигур
    /// </summary>
    /// <param name="rectTransform1"></param>
    /// <returns></returns>
    public bool IsIntersect(RectTransform rectTransform1)
    {
        if (figureRects.Count == 0) return false;

        foreach (RectTransform rectTransform2 in figureRects)
        {
            if (rectTransform1 == rectTransform2) continue;

            Rect rect1 = GetWorldRect(rectTransform1);
            Rect rect2 = GetWorldRect(rectTransform2);

            if (rect1.Overlaps(rect2))
                return true;
        }

        return false;
    }

    /// <summary>
    /// получить мировой rect
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <returns></returns>
    Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Camera camera = canvas.worldCamera ?? Camera.main;
            if (camera != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
                    corners[i] = camera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, camera.nearClipPlane));
                }
            }
        }

        Vector2 size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        return new Rect(corners[0], size);
    }
}
