using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// менеджер совпадения логотипов и фраз
/// </summary>
public class LogoManager : Speciality
{
    [SerializeField] CompareLogos compareLogos;
    [SerializeField] GameObject gameWindow;
    [SerializeField] Text _timerText;
    [SerializeField] GameObject _compareLogoWindow;
    [SerializeField] GameObject _readyBtn;
    [SerializeField] GameObject _panel;

    public static LogoManager Instance { get; private set; }

    int _countMathes = 0;
    Logo _lastLogo;
    Coroutine _timerCoroutine;
    
    void Awake()
    {
        Instance = this;
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
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsCompareLogoComplite;
    }

    /// <summary>
    /// начало игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete)
        {
            gameWindow.SetActive(true);
            _panel.SetActive(true);
            return;
        }
        compareLogos.CreateRandomLogos();
        StartTimer();
        gameWindow.SetActive(true);
    }

    /// <summary>
    /// выбрать логотип
    /// </summary>
    /// <param name="_logo"></param>
    public void SelectLogo(Logo _logo)
    {
        if (_lastLogo == null)
        {
            _lastLogo = _logo;
            _lastLogo.LightImage(Color.yellow);
        }
        else if (_lastLogo.ID == _logo.ID)
        {
            _lastLogo.MathcLogo();
            _logo.MathcLogo();
            _lastLogo = null;

            _countMathes++;
            if (_countMathes == 5)
            {
                if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
                _readyBtn.SetActive(true);

                IsComplete = true;
                SpecialityManager.Instance.Saves.SavesData.IsCompareLogoComplite = IsComplete;
                SpecialityManager.Instance.Saves.Save();
            }

        }
        else if (_lastLogo.Type != _logo.Type)
        {
            _lastLogo.WrongLogo();
            _logo.WrongLogo();
            _lastLogo = null;
        }
    }

    /// <summary>
    /// начало таймера
    /// </summary>
    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(Timer());
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
                _compareLogoWindow.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(timeStep);
        }
    }
}
