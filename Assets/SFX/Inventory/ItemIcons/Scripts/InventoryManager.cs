using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    //public List<Item> Items = new List<Item>();
    private Dictionary<Item, int> Items = new Dictionary<Item, int>();
    private GameObject obj = null;

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Transform PreviewItemContent;
    public GameObject DescriptionField;
    public GameObject TitleField;
    public GameObject ImageField;
    public GameObject BottomSeparator;

    private void Awake()
    {
        Instance = this;
        if (Items.Count == 0)
        {
            DescriptionField.SetActive(false);
            TitleField.SetActive(false);
            ImageField.SetActive(false);
            BottomSeparator.SetActive(false);
        }
    }

    public void Add(Item item)
    {
        if (Items.Count == 0)
        {
            DescriptionField.SetActive(true);
            TitleField.SetActive(true);
            ImageField.SetActive(true);
            BottomSeparator.SetActive(true);

        }
        if (Items.TryGetValue(item, out var current))
        {
            Items[item] = ++current;
        }
        else
        {
            Items.Add(item, 0);
        }
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    private void ShowItem(Item item, int count)
    {
        if (count % item.maxAmount == 0)
        {
            obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
            var itemCount = obj.transform.Find("CountNumber").GetComponent<TMP_Text>();
            var button = obj.transform.gameObject.GetComponent<Button>();

            button.onClick.AddListener(delegate { PressButton(item); }); ;
            itemCount.text = $"{count % item.maxAmount + 1}";
            itemIcon.sprite = item.icon;
        }
        else
        {
            var itemCount = obj.transform.Find("CountNumber").GetComponent<TMP_Text>();
            itemCount.text = $"{count % item.maxAmount + 1}";

        }
    }

    public void ListItems()
    {
        foreach (var item in Items)
        {
            ShowItem(item.Key, item.Value);
        }
    }

    public void PressButton(Item item)
    {
        var itemDescription = DescriptionField.transform.GetComponent<TMP_Text>();
        var itemTitle = TitleField.transform.GetComponent<TMP_Text>();
        var itemImage = ImageField.transform.GetComponent<Image>();

        itemDescription.text = item.description;
        itemTitle.text = item.title;
        itemImage.sprite = item.icon;
    }

}
