using UnityEngine;

public class CraftItemIcon : MonoBehaviour
{
    public Sprite firstIcon = null;
    public int firstAmount = 0;
    public Sprite secondIcon = null;
    public int secondAmount = 0;
    public Sprite thirdIcon = null;
    public int thirdAmount = 0;

    public Transform ItemContent;
    public GameObject InventoryItem;
    public Item item;


    public int firstId = -1;
    public int secondId = -1;
    public int thirdId = -1;
}
