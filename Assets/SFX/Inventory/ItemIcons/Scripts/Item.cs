using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public int maxAmount;
    public Sprite icon;
}
