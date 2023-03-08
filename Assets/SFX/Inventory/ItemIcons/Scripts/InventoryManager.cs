using System.Collections.Generic;
using System.Linq;
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

    private StackPosition[] stackPositions;

    public Image progressBar = null;
    public Button craftButton = null;

    private int currentPage = 0;
    public bool crafted = false;

    public bool canBeCrafted = false;

    private void Awake()
    {
        stackPositions = FindObjectsOfType<StackPosition>();

        Instance = this;
        if (fPSController.isInInventoryView)
        {
            InventoryGridContent.gameObject.transform.localScale = new Vector3(1, 1, 1);
            CraftingContent.gameObject.transform.localScale = Vector3.zero;
        }
        if (Items.Count == 0)
        {
            DescriptionField.SetActive(false);
            TitleField.SetActive(false);
            ImageField.SetActive(false);
            BottomSeparator.SetActive(false);
        }

        progressBar.gameObject.GetComponent<Image>().fillAmount = 0;

    }

    private void Update()
    {
        stackPositions = FindObjectsOfType<StackPosition>();

        var toBeCrafted = true;

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryGridContent.gameObject.transform.localScale = new Vector3(1, 1, 1);
            CraftingContent.gameObject.transform.localScale = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentPage == 0)
            {
                InventoryGridContent.gameObject.transform.localScale = Vector3.zero;
                CraftingContent.gameObject.transform.localScale = new Vector3(1, 1, 1);
                currentPage = 1;
            }
            else
            {
                InventoryGridContent.gameObject.transform.localScale = new Vector3(1, 1, 1);
                CraftingContent.gameObject.transform.localScale = Vector3.zero;
                currentPage = 0;
            }
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

            if (firstItem && secondItem && thirdItem && carouselView && carouselView.CraftItemIcon)
            {

                if (carouselView.CraftItemIcon.firstIcon != null)
                {
                    firstIcon.sprite = carouselView.CraftItemIcon.firstIcon;
                    firstNN.text = carouselView.CraftItemIcon.firstAmount.ToString();
                    firstCN.text = ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.firstId).Count().ToString();
                    if (ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.firstId).Count() < carouselView.CraftItemIcon.firstAmount)
                    {
                        toBeCrafted = false;
                    }
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
                    secondCN.text = ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.secondId).Count().ToString();
                    if (ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.secondId).Count() < carouselView.CraftItemIcon.secondAmount)
                    {
                        toBeCrafted = false;
                    }
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
                    thirdCN.text = ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.thirdId).Count().ToString();
                    if (ItemsPrev.Where(item => item.id == carouselView.CraftItemIcon.thirdId).Count() < carouselView.CraftItemIcon.thirdAmount)
                    {
                        toBeCrafted = false;
                    }
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
            if (toBeCrafted)
            {
                canBeCrafted = true;
            }
            else
            {
                canBeCrafted = false;
            }
        }
        if (canBeCrafted)
        {
            craftButton.gameObject.SetActive(true);
        }
        else
        {
            progressBar.gameObject.GetComponent<Image>().fillAmount = 0;
            craftButton.gameObject.SetActive(false);
        }

        if (crafted)
        {
            HandleCraft();
            crafted = false;
        }
    }

    private StackPosition getSmallestCurrentValuePos(int id, int minVal)
    {
        StackPosition pos = null;
        foreach (StackPosition obj in stackPositions)
        {
            if (id != -1)
            {
                if (obj.itemId == carouselView.CraftItemIcon.firstId)
                {
                    if (obj.currentValue < minVal)
                    {
                        pos = obj;
                        minVal = obj.currentValue;
                    }
                }
            }
        }
        return pos;
    }

    private void HandleRemoveFromStack(StackPosition pos, int amount)
    {
        print(pos.currentValue + "  " + amount);
        if (amount >= pos.currentValue)
        {
            Destroy(pos.gameObject);
        }
    }

    public void HandleCraft()
    {
        int minVal = 21;
        StackPosition lastStackFirstPos = getSmallestCurrentValuePos(carouselView.CraftItemIcon.firstId, minVal);
        StackPosition lastStackSecondPos = getSmallestCurrentValuePos(carouselView.CraftItemIcon.secondId, minVal);
        StackPosition lastStackThirdPos = getSmallestCurrentValuePos(carouselView.CraftItemIcon.thirdId, minVal);

        HandleRemoveFromStack(lastStackFirstPos, carouselView.CraftItemIcon.firstAmount);
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

        ItemsPrev.Add(item);

        var inStack = InStack(stackPositions, item);

        if (Items.TryGetValue(item, out var current))
        {

            Items[item] = ++current;
            if (Items[item] >= inStack.maxAmount)
            {
                inStack.isFull = true;
                Items[item] = 0;
                NewItem(item, 0);
            }
            else
            {
                if (inStack != null)
                {
                    if (current < inStack.maxAmount)
                    {
                        var itemCount = inStack.transform.Find("CountNumber").GetComponent<TMP_Text>();
                        itemCount.text = $"{current % item.maxAmount + 1}";
                        inStack.currentValue = current % item.maxAmount + 1;
                    }
                    else
                    {
                        Items[item] = ++current;
                        NewItem(item, 0);
                    }
                }
            }
        }
        else
        {
            Items.Add(item, 0);
            NewItem(item, 0);
        }
    }

    public StackPosition InStack(StackPosition[] stackPositions, Item item)
    {
        StackPosition ret = null;
        foreach (StackPosition obj in stackPositions)
        {
            if (obj.itemId == item.id && !obj.isFull)
            {
                ret = obj;
                break;
            }
        }

        return ret;
    }

    private void NewItem(Item item, int count)
    {
        obj = Instantiate(InventoryItem, ItemContent);
        var itemStackPos = obj.GetComponent<StackPosition>();
        var itemIcon = obj.transform.Find("Icon").GetComponent<Image>();
        var itemCount = obj.transform.Find("CountNumber").GetComponent<TMP_Text>();
        var button = obj.transform.gameObject.GetComponent<Button>();

        button.onClick.AddListener(delegate { PressButton(item); }); ;
        itemCount.text = $"{count % item.maxAmount + 1}";
        itemIcon.sprite = item.icon;
        itemStackPos.id = itemStackPos.GetInstanceID();
        itemStackPos.maxAmount = item.maxAmount;
        itemStackPos.isFull = false;
        itemStackPos.itemId = item.id;
        itemStackPos.currentValue = 1;
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
