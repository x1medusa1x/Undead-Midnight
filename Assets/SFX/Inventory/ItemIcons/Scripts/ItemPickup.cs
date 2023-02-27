using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        InventoryManager.Instance.ListItems();

    }

    public void OnMouseDown()
    {
        Pickup();
    }
}
