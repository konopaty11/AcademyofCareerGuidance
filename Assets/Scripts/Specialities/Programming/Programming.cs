using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� �������� ����
/// </summary>
public class Programming : Speciality
{
    [SerializeField] GameObject gameWindow;
    [SerializeField] Text _timerText;
    [SerializeField] GameObject miniGame; 
    [SerializeField] InputField inputField;
    [SerializeField] Text codeText;
    [SerializeField] RectTransform codeTransform;
    [SerializeField] GameObject _panel;

    string code;

    Image inputFieldImage;
    Coroutine _timerCoroutine;

    int countCodes = 0;

    void OnEnable()
    {
        Saves.SavesLoad += RestoreSettings;
    }

    void OnDisable()
    {
        Saves.SavesLoad -= RestoreSettings;
    }

    void Start()
    {
        inputFieldImage = inputField.GetComponent<Image>();
    }

    /// <summary>
    /// �������� ��������
    /// </summary>
    void RestoreSettings()
    {
        IsComplete = SpecialityManager.Instance.Saves.SavesData.IsPasswordComplite;
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    void ResetGame()
    {
        codeTransform.gameObject.SetActive(true);
        code = "";
        inputFieldImage.color = Color.gray;
        inputField.text = "";
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public void StartGame()
    {
        if (IsComplete)
        {
            gameWindow.SetActive(true);
            _panel.SetActive(true);
            return;
        }
        gameWindow.SetActive(true);
        ResetGame();
        StartCoroutine(ShowCode());

        _timerCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// �������� ���
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowCode()
    {
        ResetGame();

        for (int i = 0; i < 6; i++)
            code += Random.Range(0, 10).ToString();

        codeText.text = code;

        codeTransform.anchoredPosition = new(
            Random.Range(-(Screen.width - codeTransform.sizeDelta.x) / 2, (Screen.width - codeTransform.sizeDelta.x) / 2),
            Random.Range(-(Screen.height - codeTransform.sizeDelta.y) / 2, (Screen.height - codeTransform.sizeDelta.y) / 2)
            );

        yield return new WaitForSeconds(2f);

        codeTransform.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��������� 1 � ���� �����
    /// </summary>
    public void OneClick()
    {
        inputField.text += "1";
    }

    /// <summary>
    /// ��������� 2 � ���� �����
    /// </summary>
    public void TwoClick()
    {
        inputField.text += "2";
    }

    /// <summary>
    /// ��������� 3 � ���� �����
    /// </summary>
    public void ThreeClick()
    {
        inputField.text += "3";
    }

    /// <summary>
    /// ��������� 4 � ���� �����
    /// </summary>
    public void FourClick()
    {
        inputField.text += "4";
    }

    /// <summary>
    /// ��������� 5 � ���� �����
    /// </summary>
    public void FiveClick()
    {
        inputField.text += "5";
    }

    /// <summary>
    /// ��������� 6 � ���� �����
    /// </summary>
    public void SixClick()
    {
        inputField.text += "6";
    }

    /// <summary>
    /// ��������� 7 � ���� �����
    /// </summary>
    public void SevenClick()
    {
        inputField.text += "7";
    }

    /// <summary>
    /// ��������� 8 � ���� �����
    /// </summary>
    public void EightClick()
    {
        inputField.text += "8";
    }

    /// <summary>
    /// ��������� 9 � ���� �����
    /// </summary>
    public void NineClick()
    {
        inputField.text += "9";
    }

    /// <summary>
    /// ��������� 0 � ���� �����
    /// </summary>
    public void ZeroClick()
    {
        inputField.text += "0";
    }

    /// <summary>
    /// ��������� ���
    /// </summary>
    public void Confirm()
    {
        countCodes++;

        if (inputField.text == code)
            inputFieldImage.color = Color.green;
        else
            inputFieldImage.color = Color.red;

        if (countCodes == 5)
            StartCoroutine(CloseGame());
        else
            StartCoroutine(NextCode());
    }

    /// <summary>
    /// ������� � ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator NextCode()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowCode());
    }

    /// <summary>
    /// �������� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseGame()
    {
        StopCoroutine(_timerCoroutine);
        yield return new WaitForSeconds(1f);
        miniGame.SetActive(false);

        countCodes = 0;
        IsComplete = true;
        SpecialityManager.Instance.Saves.SavesData.IsPasswordComplite = IsComplete;
        SpecialityManager.Instance.Saves.Save();
    }

    /// <summary>
    /// ������� ������ �� �����
    /// </summary>
    public void Delete()
    {
        if (inputField.text.Length != 0)
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    /// <summary>
    /// ������ �� ����
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
                miniGame.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(timeStep);
        }
    }

}
