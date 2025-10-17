using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// фоторобот
/// </summary>
public class Identikit : Speciality
{
    [SerializeField] CaptureScreen captureScreen;
    [SerializeField] RectTransform captureArea;

    [Header("ChoseItems")]
    [SerializeField] ChoseItem eyes;
    [SerializeField] ChoseItem face;
    [SerializeField] ChoseItem hair;
    [SerializeField] ChoseItem beard;
    [SerializeField] ChoseItem glasses;
    [SerializeField] ChoseItem lips;
    [SerializeField] ChoseItem nose;

    [SerializeField] Image image;
    [SerializeField] List<SpriteSerelizable> identikits;
    [SerializeField] Text _timerText;
    [SerializeField] GameObject _timer;
    [SerializeField] GameObject scrollView;

    [SerializeField] GameObject gameWindow;
    [SerializeField] GameObject startGameBtn;
    [SerializeField] GameObject readyBtn;
    [SerializeField] GameObject faceItems;
    [SerializeField] GameObject identikit;

    Coroutine _timerCoroutine;
    List<FaceItem> choseItems = new();
    List<FaceItem> targetItems;

    /// <summary>
    /// класс сериализованного спрайта
    /// </summary>

    [System.Serializable]
    class SpriteSerelizable
    {
        public Sprite sprite;
        public List<FaceItem> items;
    }

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
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsIdentikitComplite;
    }
    /// <summary>
    /// началао игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete) return;
        gameWindow.SetActive(true);
        SetIdentikit();
    }

    /// <summary>
    /// установить фоторобот
    /// </summary>
    public void SetIdentikit()
    {
        ResetMiniGame();

        int index = Random.Range(0, identikits.Count);
        image.sprite = identikits[index].sprite;
        targetItems = identikits[index].items;
        targetItems.Sort();

        _timerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// начало составления фоторобота
    /// </summary>
    public void StartMiniGame()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        image.gameObject.SetActive(false);
        _timer.gameObject.SetActive(false);
        scrollView.SetActive(true);
    }

    /// <summary>
    /// таймер
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        float allTime = 30f;
        float currentTime = allTime;
        float timeStep = 1f;

        while (true)
        {
            _timerText.text = currentTime.ToString();
            currentTime -= timeStep;

            if (currentTime < 0)
            {
                StartMiniGame();
                yield break;
            }
            yield return new WaitForSeconds(timeStep);
        }
    }

    /// <summary>
    /// сравнить элеменнты лица
    /// </summary>
    public void MatchFaceItems()
    {
        StartCoroutine(CaptureCoroutime());
    }

    /// <summary>
    /// ссравнение фотороботов
    /// </summary>
    void Match()
    {
        choseItems.Add(eyes.CurrentFaceItem);
        choseItems.Add(hair.CurrentFaceItem);
        choseItems.Add(face.CurrentFaceItem);
        choseItems.Add(nose.CurrentFaceItem);
        choseItems.Add(lips.CurrentFaceItem);
        choseItems.Add(beard.CurrentFaceItem);
        choseItems.Add(glasses.CurrentFaceItem);

        for (int i = 0; i < choseItems.Count;)
        {
            if (choseItems[i] == FaceItem.None)
                choseItems.RemoveAt(i);
            else
                i++;
        }
        choseItems.Sort();


        if (choseItems.SequenceEqual(targetItems))
        {
            foreach (FaceItem item in targetItems)
                RightItem(item);
        }

        foreach (FaceItem item in choseItems)
        {
            if (!targetItems.Contains(item))
                WrongItem(item);
        }

        foreach (FaceItem item in targetItems)
        {
            if (!choseItems.Contains(item))
                WrongItem(item);
        }

        StartCoroutine(CloseMiniGame());
        IsComplete = true;
        SpecialityManager.Instance.Saves.SavesData.IsIdentikitComplite = IsComplete;
        SpecialityManager.Instance.Saves.Save();

        choseItems = new();
    }

    /// <summary>
    /// корутина скриншота составленного фоторобота
    /// </summary>
    /// <returns></returns>
    IEnumerator CaptureCoroutime()
    {
        CaptureIdentikit();
        yield return new WaitForEndOfFrame();
        Match();
    }

    /// <summary>
    /// закрытие окна игры
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseMiniGame()
    {
        yield return new WaitForSeconds(2f);
        gameWindow.SetActive(false);
    }

    /// <summary>
    /// неправильный выбор
    /// </summary>
    /// <param name="item"></param>
    void WrongItem(FaceItem item)
    {
        // шаблонные выражения
        switch (item)
        {
            case >= FaceItem.Hair_1 and <= FaceItem.Hair_4:
                hair.LightRed();
                break;

            case >= FaceItem.Face_1 and <= FaceItem.Face_4:
                face.LightRed();
                break;

            case >= FaceItem.Lip_1 and <= FaceItem.Lip_4:
                lips.LightRed();
                break;

            case >= FaceItem.Nose_1 and <= FaceItem.Nose_4:
                nose.LightRed();
                break;

            case >= FaceItem.Beard_1 and <= FaceItem.Beard_4:
                beard.LightRed();
                break;

            case >= FaceItem.Glasses_1 and <= FaceItem.Glasses_4:
                glasses.LightRed();
                break;

            case >= FaceItem.Eye_1 and <= FaceItem.Eye_4:
                eyes.LightRed();
                break;
        }
    }

    /// <summary>
    /// правильный выбор
    /// </summary>
    /// <param name="item"></param>
    void RightItem(FaceItem item)
    {
        // шаблонные выражения
        switch (item)
        {
            case >= FaceItem.Hair_1 and <= FaceItem.Hair_4:
                hair.LightGreen();
                break;

            case >= FaceItem.Face_1 and <= FaceItem.Face_4:
                face.LightGreen();
                break;

            case >= FaceItem.Lip_1 and <= FaceItem.Lip_4:
                lips.LightGreen();
                break;

            case >= FaceItem.Nose_1 and <= FaceItem.Nose_4:
                nose.LightGreen();
                break;

            case >= FaceItem.Beard_1 and <= FaceItem.Beard_4:
                beard.LightGreen();
                break;

            case >= FaceItem.Glasses_1 and <= FaceItem.Glasses_4:
                glasses.LightGreen();
                break;

            case >= FaceItem.Eye_1 and <= FaceItem.Eye_4:
                eyes.LightGreen();
                break;
        }
    }

    /// <summary>
    /// сбросить игру
    /// </summary>
    public void ResetMiniGame()
    {
        eyes.ResetImage();
        face.ResetImage();
        hair.ResetImage();
        beard.ResetImage();
        glasses.ResetImage();
        lips.ResetImage();
        nose.ResetImage();

        startGameBtn.SetActive(true);
        readyBtn.SetActive(false);
        faceItems.SetActive(false);
        identikit.SetActive(true);
        _timer.gameObject.SetActive(true);
    }

    /// <summary>
    /// скриншот фоторобота
    /// </summary>
    void CaptureIdentikit()
    {
        Vector3[] corners = new Vector3[4];
        captureArea.GetWorldCorners(corners);

        int width = (int)(corners[2].x - corners[0].x);
        int height = (int)(corners[2].y - corners[0].y);
        int x = (int)corners[0].x;
        int y = (int)corners[0].y;

        captureScreen.CaptureAreaAsSprite(width, height, x, y);

    }
}
