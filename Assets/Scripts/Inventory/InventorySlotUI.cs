using UnityEngine;
using IJunior.Core.UI.Dragging;

namespace IJunior.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer
    {
        [SerializeField] InventoryItemIcon icon = null;

        public void AddItem(Sprite item)
        {
            icon.SetItem(item);
        }

        public Sprite GetItem()
        {
            return icon.GetItem();
        }

        public void RemoveItem()
        {
            icon.SetItem(null);
        }
    }
}