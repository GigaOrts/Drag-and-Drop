using UnityEngine;

namespace IJunior.Core.UI.Dragging
{
    public interface IDragSource
    {
        Sprite GetItem();
        void RemoveItem();
    }
}