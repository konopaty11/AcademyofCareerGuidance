using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropdownController : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    [SerializeField] List<ItemsSerializable> items;

    [System.Serializable]
    class ItemsSerializable
    {
        public int index;
        public GameObject scroll;
    }

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(int index)
    {
        foreach (ItemsSerializable item in items)
        {
            item.scroll.SetActive(false);
        }

        foreach (ItemsSerializable item in items)
        {
            if (item.index == index)
                item.scroll.SetActive(true);
        }
    }
}
