using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������
/// </summary>
public class Logo : MonoBehaviour
{
    [SerializeField] LogoName type;
    [SerializeField] LogoID _id;
    [SerializeField] Image image;

    public LogoID ID => _id;
    public LogoName Type => type;

    /// <summary>
    /// ������� �������
    /// </summary>
    public void Select()
    {
        LogoManager.Instance.SelectLogo(this);
    }
    
    /// <summary>
    /// ��������� ����� �����������
    /// </summary>
    /// <param name="color"></param>
    public void LightImage(Color color)
    {
        image.color = color;
    }

    /// <summary>
    /// ���������� �������� � �����
    /// </summary>
    public void MathcLogo()
    {
        StartCoroutine(LightGreen());
    }

    /// <summary>
    /// ������������ �������� � �����
    /// </summary>
    public void WrongLogo()
    {
        StartCoroutine(LightRed());
    }

    /// <summary>
    /// ������� �����������
    /// </summary>
    /// <returns></returns>
    IEnumerator LightRed()
    {
        LightImage(Color.red);
        yield return new WaitForSeconds(1f);
        LightImage(Color.clear);
    }

    /// <summary>
    /// ������� �����������
    /// </summary>
    /// <returns></returns>
    IEnumerator LightGreen()
    {
        LightImage(Color.green);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
