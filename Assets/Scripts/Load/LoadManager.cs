using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField] Text _textProgress;
    [SerializeField] Slider _progressBar;
    [SerializeField] GameObject _loadWindow;

    const int _firstSceneIndex = 0;
    const string _patternText = "Загрузка: ";

    public static LoadManager Instance { get; private set; } 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartCoroutine(LoadScreen());
    }

    /// <summary>
    /// отображает прогресс загрузки в виде загрузочного окна
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScreen()
    {
        //_loadWindow.SetActive(true);

        //AsyncOperation _operation = SceneManager.LoadSceneAsync(_firstSceneIndex);
        //_operation.allowSceneActivation = false;

        //while (!_operation.isDone)
        //{
        //    float _progress = _operation.progress / 0.9f;

        //    _progressBar.value = _progress;
        //    _textProgress.text = _patternText + (_progress * 100) + "%";

        //    if (_progress >= 0.99f)
        //    {
        //        _textProgress.text = _patternText + 100 + "%";
        //        _operation.allowSceneActivation = true;

        //        break;
        //    }

        //    yield return null;
        //}

        //_loadWindow.SetActive(false);

        _loadWindow.SetActive(true);

        float duration = 2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float _progress = Mathf.Lerp(0f, duration, elapsed / duration);

            _progressBar.value = _progress / 2;
            _textProgress.text = _patternText + (int)(_progress * 50) + "%";

            yield return null;
        }

        _loadWindow.SetActive(false);
    }
}