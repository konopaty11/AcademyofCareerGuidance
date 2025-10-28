using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// захват скрина
/// </summary>
public class CaptureScreen : MonoBehaviour
{
    [SerializeField] CreateLogo createLogo;

    int left = -1;
    int right;
    int top;
    int bottom = -1;

    /// <summary>
    /// вырезать фигуры
    /// </summary>
    public void CaptureFiguresAsSprite()
    {
        SetCaptureArea();
        StartCoroutine(CaptureCoroutine(right - left, top - bottom, left, bottom));
    }

    /// <summary>
    /// вырезать площадь экрана
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CaptureAreaAsSprite(int width, int height, int x, int y)
    {
        StartCoroutine(CaptureCoroutine(width, height, x, y));
    }

    /// <summary>
    /// определение координат захвата фигур
    /// </summary>
    void SetCaptureArea()
    {
        foreach (GameObject figureObject in createLogo.Figures)
        {
            Figure figure = figureObject.GetComponent<Figure>();

            RectTransform rectTransform = figureObject.GetComponent<RectTransform>();

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            Camera camera = canvas?.worldCamera ?? Camera.main;

            int leftFigure = int.MaxValue;
            int rightFigure = int.MinValue;
            int topFigure = int.MinValue;
            int bottomFigure = int.MaxValue;

            foreach (Vector3 corner in corners)
            {
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, corner);

                leftFigure = Mathf.Min(leftFigure, (int)screenPoint.x);
                rightFigure = Mathf.Max(rightFigure, (int)screenPoint.x);
                topFigure = Mathf.Max(topFigure, (int)screenPoint.y);
                bottomFigure = Mathf.Min(bottomFigure, (int)screenPoint.y);
            }

            left = (left == -1) ? leftFigure : Mathf.Min(left, leftFigure);
            right = Mathf.Max(right, rightFigure);
            top = Mathf.Max(top, topFigure);
            bottom = (bottom == -1) ? bottomFigure : Mathf.Min(bottom, bottomFigure);
        }
    }

    /// <summary>
    /// корутина захвата экрана
    /// </summary>
    /// <param name="captureWidth"></param>
    /// <param name="captureHeight"></param>
    /// <param name="captureX"></param>
    /// <param name="captureY"></param>
    /// <returns></returns>
    IEnumerator CaptureCoroutine(int captureWidth, int captureHeight, int captureX, int captureY)
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(captureX, captureY, captureWidth, captureHeight), 0, 0);
        texture.Apply();

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, captureWidth, captureHeight),
            new Vector2(0.5f, 0.5f),
            100f
        );

        GalleryManager.Instance.SetNextSprite(sprite);
    }
}
