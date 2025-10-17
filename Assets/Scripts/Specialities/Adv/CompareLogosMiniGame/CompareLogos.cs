using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// игра сравнить логотипы
/// </summary>
public class CompareLogos : MonoBehaviour
{
    [SerializeField] List<RectTransform> _logosTransform;
    [SerializeField] List<GameObject> _logoPrefabs;
    [SerializeField] List<RectTransform> _namesTransform;
    [SerializeField] List<GameObject> _namePrefabs;

    List<RectTransform> _currentLogosTransform;
    List<RectTransform> _currentNamesTransform;

    /// <summary>
    /// создать рандомный набор логотип и фраз
    /// </summary>
    public void CreateRandomLogos()
    {
        _currentLogosTransform = new();
        _currentNamesTransform = new();

        foreach (RectTransform rect in _logosTransform)
            _currentLogosTransform.Add(rect);

        foreach (RectTransform rect in _namesTransform)
            _currentNamesTransform.Add(rect);

        foreach (GameObject logoPrefab in _logoPrefabs)
        {
            RectTransform _rect = _currentLogosTransform[Random.Range(0, _currentLogosTransform.Count)];
            GameObject logo = Instantiate(logoPrefab, _rect);

            _currentLogosTransform.Remove(_rect);
        }

        foreach (GameObject namePrefab in _namePrefabs)
        {
            RectTransform _rect = _currentNamesTransform[Random.Range(0, _currentNamesTransform.Count)];
            GameObject name = Instantiate(namePrefab, _rect);

            _currentNamesTransform.Remove(_rect);
        }
    }
}
