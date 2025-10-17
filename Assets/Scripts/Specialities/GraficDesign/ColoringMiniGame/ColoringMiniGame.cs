using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// игра раскраски изображения
/// </summary>
public class ColoringMiniGame : Speciality, IPointerClickHandler
{
    [SerializeField] GameObject readyBtn;
    [SerializeField] GameObject gameWindow;

    [Header("Настройки")]
    [SerializeField] Image targetImage;       
    [SerializeField] Color currentColor;     

    Texture2D originalTexture;  
    Texture2D workTexture;      
    Sprite originalSprite;
    Vector2 uv;

    bool isColorSelected = false;
    bool isUVDecided = false;
    void Start()
    {
        originalSprite = targetImage.sprite;
        originalTexture = originalSprite.texture;

        workTexture = new Texture2D(originalTexture.width, originalTexture.height);
        workTexture.SetPixels(originalTexture.GetPixels());
        workTexture.Apply();

        ApplyTextureToImage();
    }

    /// <summary>
    /// поставить текущий цвет
    /// </summary>
    /// <param name="color"></param>
    public void SetCurrentColor(Color color)
    {
        currentColor = color;
        isColorSelected = true;
    }

    /// <summary>
    /// закраска области
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetImage.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            uv = ConvertToTextureUV(localPoint);
            isUVDecided = true;
        }
    }

    /// <summary>
    /// конвертация координат нажатия в координаты текстуры
    /// </summary>
    Vector2 ConvertToTextureUV(Vector2 localPoint)
    {
        Rect rect = targetImage.rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;
        return new Vector2(x, y);
    }

    public IEnumerator StartFill(Color choseColor)
    {
        while (!isUVDecided)
            yield return null;

        isUVDecided = false;
        FloodFill(uv, choseColor);

        if (ClickArea.CountAreasClicked == 5)
        {
            IsComplete = true;
            SpecialityManager.Instance.Saves.SavesData.IsColoringComplite = IsComplete;
            SpecialityManager.Instance.Saves.Save();
            readyBtn.SetActive(true);
        }
    }

    /// <summary>
    /// заливка области
    /// </summary>
    void FloodFill(Vector2 startUV, Color choseColor)
    {
        if (!isColorSelected || currentColor != choseColor) return;

        int startX = (int)(startUV.x * workTexture.width);
        int startY = (int)(startUV.y * workTexture.height);

        if (!CanFillHere(startX, startY)) return;

        Color targetColor = workTexture.GetPixel(startX, startY);

        if (targetColor == currentColor) return;

        Queue<Vector2Int> pixels = new Queue<Vector2Int>();
        pixels.Enqueue(new Vector2Int(startX, startY));

        while (pixels.Count > 0)
        {
            Vector2Int point = pixels.Dequeue();
            int x = point.x;
            int y = point.y;

            if (x < 0 || x >= workTexture.width || y < 0 || y >= workTexture.height)
                continue;

            Color pixelColor = workTexture.GetPixel(x, y);

            if (IsOutline(pixelColor) || pixelColor == currentColor)
                continue;

            if (pixelColor.a < 0.1f || pixelColor == targetColor)
            {
                workTexture.SetPixel(x, y, currentColor);

                pixels.Enqueue(new Vector2Int(x + 1, y)); 
                pixels.Enqueue(new Vector2Int(x - 1, y)); 
                pixels.Enqueue(new Vector2Int(x, y + 1)); 
                pixels.Enqueue(new Vector2Int(x, y - 1)); 
            }
        }

        workTexture.Apply();
        ApplyTextureToImage();
    }

    /// <summary>
    /// проверка заливки
    /// </summary>
    private bool CanFillHere(int x, int y)
    {
        if (x < 0 || x >= workTexture.width || y < 0 || y >= workTexture.height)
            return false;

        Color pixel = workTexture.GetPixel(x, y);

        return !IsOutline(pixel);
    }

    /// <summary>
    /// контур ли пиксель
    /// </summary>
    private bool IsOutline(Color color)
    {
        return color.a > 0.1f; 
    }

    /// <summary>
    /// применение тексиуры к изображению
    /// </summary>
    private void ApplyTextureToImage()
    {
        Sprite newSprite = Sprite.Create(
            workTexture,
            originalSprite.rect,
            originalSprite.pivot,
            originalSprite.pixelsPerUnit
        );
        targetImage.sprite = newSprite;
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
        GalleryManager.Instance.SetNextSprite(targetImage.sprite);
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForEndOfFrame();
        gameWindow.SetActive(false);
    }
}
