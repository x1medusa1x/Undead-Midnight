using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public FPSController fPSController = null;

    public Canvas InventoryCanvas;
    public CarouselView carouselView = null;

    public Sprite emptySlot = null;

    public Dictionary<Item, int> Items = new Dictionary<Item, int>();
    public List<Item> ItemsPrev = null;
    private GameObject obj = null;

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Transform PreviewItemContent;
    public GameObject InventoryGridContent;
    public GameObject CraftingContent;
    public GameObject DescriptionField;
    public GameObject TitleField;
    public GameObject ImageField;
    public GameObject BottomSeparator;

    private void Awake()
    {
        Instance = this;
        if (fPSController.isInInventoryView)
        {
            InventoryGridContent.gameObject.SetActive(true);
            CraftingContent.gameObject.SetActive(false);
        }
        if (Items.Count == 0)
        {
            DescriptionField.SetActive(false);
            TitleField.SetActive(false);
            ImageField.SetActive(false);
            BottomSeparator.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryGridContent.gameObject.SetActive(true);
            CraftingContent.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            InventoryGridContent.gameObject.SetActive(!InventoryGridContent.gameObject.activeSelf);
            CraftingContent.gameObject.SetActive(!CraftingContent.gameObject.activeSelf);
        }
        if (GameObject.Find("CraftingContent"))
        {
            var firstItem = GameObject.Find("CraftingContent").transform.Find("Triangle").transform.Find("FirstItem");
            var secondItem = GameObject.Find("CraftingContent").transform.Find("Triangle").transform.Find("SecondItem");
            var thirdItem = GameObject.Find("CraftingContent").transform.Find("Triangle").transform.Find("ThirdItem");

            var firstIcon = firstItem.Find("Icon").gameObject.GetComponent<Image>();
            var secondIcon = secondItem.Find("Icon").gameObject.GetComponent<Image>();
            var thirdIcon = thirdItem.Find("Icon").gameObject.GetComponent<Image>();

            var firstC = firstItem.Find("Count").gameObject.GetComponent<Image>();
            var secondC = secondItem.Find("Count").gameObject.GetComponent<Image>();
            var thirdC = thirdItem.Find("Count").gameObject.GetComponent<Image>();

            var firstCN = firstItem.Find("CountNumber").gameObject.GetComponent<TMP_Text>();
            var secondCN = secondItem.Find("CountNumber").gameObject.GetComponent<TMP_Text>();
            var thirdCN = thirdItem.Find("CountNumber").gameObject.GetComponent<TMP_Text>();

            var firstNN = firstItem.Find("NeededNumber").gameObject.GetComponent<TMP_Text>();
            var secondNN = secondItem.Find("NeededNumber").gameObject.GetComponent<TMP_Text>();
            var thirdNN = thirdItem.Find("NeededNumber").gameObject.GetComponent<TMP_Text>();

            var firstSlash = firstItem.Find("Slash").gameObject.GetComponent<TMP_Text>();
            var secondSlash = secondItem.Find("Slash").gameObject.GetComponent<TMP_Text>();
            var thirdSlash = thirdItem.Find("Slash").gameObject.GetComponent<TMP_Text>();

            print(JsonConvert.SerializeObject(Items));

            if (firstItem && secondItem && thirdItem && carouselView && carouselView.CraftItemIcon)
            {

                if (carouselView.CraftItemIcon.firstIcon != null)
                {
                    firstIcon.sprite = carouselView.CraftItemIcon.firstIcon;
                    firstNN.text = carouselView.CraftItemIcon.firstAmount.ToString();
                    firstC.gameObject.SetActive(true);
                    firstNN.gameObject.SetActive(true);
                    firstCN.gameObject.SetActive(true);
                    firstSlash.gameObject.SetActive(true);
                }
                else
                {
                    firstIcon.sprite = emptySlot;
                    firstC.gameObject.SetActive(false);
                    firstNN.gameObject.SetActive(false);
                    firstCN.gameObject.SetActive(false);
                    firstSlash.gameObject.SetActive(false);
                }
                if (carouselView.CraftItemIcon.secondIcon != null)
                {
                    secondIcon.sprite = carouselView.CraftItemIcon.secondIcon;
                    secondNN.text = carouselView.CraftItemIcon.secondAmount.ToString();
                    secondC.gameObject.SetActive(true);
                    secondNN.gameObject.SetActive(true);
                    secondCN.gameObject.SetActive(true);
                    secondSlash.gameObject.SetActive(true);
                }
                else
                {
                    secondIcon.sprite = emptySlot;
                    secondC.gameObject.SetActive(false);
                    secondNN.gameObject.SetActive(false);
                    secondCN.gameObject.SetActive(false);
                    secondSlash.gameObject.SetActive(false);
                }
                if (carouselView.CraftItemIcon.thirdIcon != null)
                {
                    thirdIcon.sprite = carouselView.CraftItemIcon.thirdIcon;
                    thirdNN.text = carouselView.CraftItemIcon.thirdAmount.ToString();
                    thirdC.gameObject.SetActive(true);
                    thirdNN.gameObject.SetActive(true);
                    thirdCN.gameObject.SetActive(true);
                    thirdSlash.gameObject.SetActive(true);
                }
                else
                {
                    thirdIcon.sprite = emptySlot;
                    thirdC.gameObject.SetActive(false);
                    thirdNN.gameObject.SetActive(false);
                    thirdCN.gameObject.SetActive(false);
                    thirdSlash.gameObject.SetActive(false);
                }
            }
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
            print(Items[item] + "    " + JsonConvert.SerializeObject(Items));
        }
        else
        {
            current = 0;
            Items.Add(item, 0);
        }

        ItemsPrev.Add(item);
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
