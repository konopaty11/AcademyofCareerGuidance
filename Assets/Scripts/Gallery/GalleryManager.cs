using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] List<Image> images;

    public static GalleryManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetNextSprite(Sprite newSprite)
    {
        foreach (Image image in images)
            if (image.sprite == null)
            {
                image.sprite = newSprite;
                break;
            }
    }
}
