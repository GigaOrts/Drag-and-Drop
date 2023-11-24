using UnityEngine;

public class InventorySlotUI : MonoBehaviour, IDragContainer
{
    [SerializeField] private InventoryItemIcon _icon;

    public void AddItem(Sprite item)
    {
        _icon.SetItem(item);
    }

    public Sprite GetItem()
    {
        return _icon.GetItem();
    }

    public void RemoveItem()
    {
        _icon.SetItem(null);
    }
}