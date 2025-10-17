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

            int leftFigure = (int)figure.GetLeftPoint();
            int rightFigure = (int)figure.GetRightPoint();
            int topFigure = (int)figure.GetTopPoint();
            int bottomFigure = (int)figure.GetBottomPoint();

            left = left > leftFigure || left == -1 ? leftFigure : left;
            right = Mathf.Max(right, rightFigure);
            top = Mathf.Max(top, topFigure);
            bottom = bottom > bottomFigure || bottom == -1 ? bottomFigure : bottom;
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

        // «ахватываем фиксированную область экрана
        Texture2D texture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(captureX, captureY, captureWidth, captureHeight), 0, 0);
        texture.Apply();

        // —оздаем спрайт
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, captureWidth, captureHeight),
            new Vector2(0.5f, 0.5f),
            100f
        );

        GalleryManager.Instance.SetNextSprite(sprite);
    }
}
