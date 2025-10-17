using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// ��������� ������� ����
/// </summary>
public class ChoseItem : MonoBehaviour
{
    [SerializeField] List<Image> images;
    [SerializeField] List<Image> frames;

    public GameObject Jackdaw { get; set; }
    public FaceItem CurrentFaceItem { get; private set; } = FaceItem.None;

    /// <summary>
    /// ���������� �������
    /// </summary>
    /// <param name="item"></param>
    /// <param name="sprite"></param>
    public void SetFaceItem(FaceItem item, Sprite sprite)
    {
        CurrentFaceItem = item;

        foreach (Image image in images)
        {
            image.color = Color.white;
            image.sprite = sprite;
        }
    }
    
    /// <summary>
    /// ���������� ����������
    /// </summary>
    public void LightGreen()
    {
        foreach (Image frame in frames)
            frame.color = Color.green;
    }

    /// <summary>
    /// ������������ ���������
    /// </summary>
    public void LightRed()
    {
        foreach (Image frame in frames)
            frame.color = Color.red;
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    public void ResetImage()
    {
        foreach(Image image in images)
        {
            image.sprite = null;
            image.color = Color.clear;
        }

        foreach (Image frame in frames)
            frame.color = Color.clear;

        if (Jackdaw != null)
        {
            Jackdaw.SetActive(false);
            Jackdaw = null;
        }
    }
}
