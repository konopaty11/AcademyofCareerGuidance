using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������� ����� ����
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
        {"Red Bull ���� ���� ������", "Red Bull" },
        {"DG ������ ������", "DG" },
        {"Apple ����� 4 �����", "Apple" },
        {"Audi A4 ���� ���� ������", "Audi" },
        {"Nike ���� ������ ������", "Nike" },
        {"Monstor ���������� ������", "Monstor" },
        {"LitEnergy", "LitEnergy" },
        {"Acura ���� �����", "Acura" },
        {"Auto Motion �� � ���� ����", "Auto Motion" },
        {"Aston Martin ���� ����", "Aston Martin" }
    };

    List<string> phrases = new()
    {
        "Red Bull ���� ���� ������",
        "DG ������ ������",
        "Apple ����� 4 �����",
        "Audi A4 ���� ���� ������",
        "Nike ���� ������ ������",
        "Monstor ���������� ������",
        "LitEnergy",
        "Acura ���� �����",
        "Auto Motion �� � ���� ����",
        "Aston Martin ���� ����",
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
    /// �������� ��������
    /// </summary>
    void RestoreSettings()
    {
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsGuessBrandComplite;
    }

    /// <summary>
    /// ������ ����
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
    /// �������� �����
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
    /// ��������� ������
    /// </summary>
    public void ValidateAnswer()
    {
        StartCoroutine(Validate());

    }

    /// <summary>
    /// �������� ����������
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
    /// ����� �������
    /// </summary>
    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// ������
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
