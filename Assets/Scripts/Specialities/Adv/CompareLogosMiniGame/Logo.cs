using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// логотип
/// </summary>
public class Logo : MonoBehaviour
{
    [SerializeField] LogoName type;
    [SerializeField] LogoID _id;
    [SerializeField] Image image;

    public LogoID ID => _id;
    public LogoName Type => type;

    /// <summary>
    /// выбрать логотип
    /// </summary>
    public void Select()
    {
        LogoManager.Instance.SelectLogo(this);
    }
    
    /// <summary>
    /// изменение цвета изображения
    /// </summary>
    /// <param name="color"></param>
    public void LightImage(Color color)
    {
        image.color = color;
    }

    /// <summary>
    /// совпадение логотипа и фраза
    /// </summary>
    public void MathcLogo()
    {
        StartCoroutine(LightGreen());
    }

    /// <summary>
    /// несовпадение логотипа и фразы
    /// </summary>
    public void WrongLogo()
    {
        StartCoroutine(LightRed());
    }

    /// <summary>
    /// красный изображение
    /// </summary>
    /// <returns></returns>
    IEnumerator LightRed()
    {
        LightImage(Color.red);
        yield return new WaitForSeconds(1f);
        LightImage(Color.clear);
    }

    /// <summary>
    /// зеленое изображение
    /// </summary>
    /// <returns></returns>
    IEnumerator LightGreen()
    {
        LightImage(Color.green);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
