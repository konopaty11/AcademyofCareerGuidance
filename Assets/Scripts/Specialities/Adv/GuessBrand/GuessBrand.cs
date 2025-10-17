using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// угадать бренд игра
/// </summary>
public class GuessBrand : Speciality
{
    [SerializeField] GameObject gameWindow;
    [SerializeField] GameObject _readyBtn;
    [SerializeField] GameObject _guessBrandWindow;
    [SerializeField] Text phraseText;
    [SerializeField] InputField inputField;
    [SerializeField] Image background;
    [SerializeField] Text _timerText;

    Dictionary<string, string> phrasesBrands = new()
    {
        {"Red Bull ƒает тебе крыль€", "Red Bull" },
        {"DG крута€ одежда", "DG" },
        {"Apple айфон 4 круто", "Apple" },
        {"Audi A4 типо влад бумага", "Audi" },
        {"Nike тоже крута€ одежда", "Nike" },
        {"Monstor коричневый монстр", "Monstor" },
        {"LitEnergy", "LitEnergy" },
        {"Acura типо хонда", "Acura" },
        {"Auto Motion нн в мире авто", "Auto Motion" },
        {"Aston Martin типо бонд", "Aston Martin" }
    };

    List<string> phrases = new()
    {
        "Red Bull ƒает тебе крыль€",
        "DG крута€ одежда",
        "Apple айфон 4 круто",
        "Audi A4 типо влад бумага",
        "Nike тоже крута€ одежда",
        "Monstor коричневый монстр",
        "LitEnergy",
        "Acura типо хонда",
        "Auto Motion нн в мире авто",
        "Aston Martin типо бонд",
    };

    List<string> currentPhrases = new();


    Coroutine _timerCoroutine;

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
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsGuessBrandComplite;
    }

    /// <summary>
    /// начало игры
    /// </summary>
    public void StartGame()
    {
        if (IsComplete) return;

        StartTimer();
        gameWindow.SetActive(true);

        _readyBtn.SetActive(false);
        currentPhrases = new();

        foreach (string str in phrases)
            currentPhrases.Add(str);

        CreatePhrase();
    }


    /// <summary>
    /// создание фразы
    /// </summary>
    public void CreatePhrase()
    {
        if (currentPhrases.Count == 0)
        {
            IsComplete = true;
            SpecialityManager.Instance.Saves.SavesData.IsGuessBrandComplite = IsComplete;
            SpecialityManager.Instance.Saves.Save();

            StopCoroutine(_timerCoroutine);
            _readyBtn.SetActive(true);
            return;
        }

        phraseText.text = currentPhrases[Random.Range(0, currentPhrases.Count)];
        currentPhrases.Remove(phraseText.text);
    }

    /// <summary>
    /// валидаци€ ответа
    /// </summary>
    public void ValidateAnswer()
    {
        StartCoroutine(Validate());

    }

    /// <summary>
    /// корутина валидацмии
    /// </summary>
    /// <returns></returns>
    IEnumerator Validate()
    {
        if (IsComplete) yield break;

        if (phrasesBrands[phraseText.text].ToLower() == inputField.text.ToLower())
        {
            background.color = Color.green;
        }
        else
        {
            background.color = Color.red;
        }

        yield return new WaitForSeconds(1f);

        background.color = Color.white;
        phraseText.text = "";
        inputField.text = "";
        CreatePhrase();
    }

    /// <summary>
    /// старт таймера
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
                _guessBrandWindow.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(timeStep);
        }
    }
}
