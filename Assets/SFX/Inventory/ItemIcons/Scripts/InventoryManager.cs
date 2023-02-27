using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            GameObject objSprite = obj.transform.GetChild(0).gameObject;
            var itemName = objSprite.transform.GetComponent<TextMeshProUGUI>();
            var itemIcon = objSprite.transform.GetComponent<Image>();

            Debug.Log(JsonConvert.SerializeObject(InventoryItem));

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

}
