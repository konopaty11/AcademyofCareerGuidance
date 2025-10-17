using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// статус выполнености специальности
/// </summary>
public class SpecialityComplete : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] List<Speciality> specialities;

    void FixedUpdate()
    {
        foreach (Speciality speciality in specialities)
            if (!speciality.IsComplete)
                return;

        image.color = Color.green;
    }
}
