using UnityEngine;
using IJunior.Core.UI.Dragging;
using UnityEngine.EventSystems;

namespace IJunior.UI.Inventories
{
    public class InventoryDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Vector3 startPosition;
        Transform originalParent;
        IDragSource source;

        Canvas parentCanvas;

        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            source = GetComponentInParent<IDragSource>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            startPosition = transform.position;
            originalParent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(originalParent, true);

            IDragDestination container;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                container = parentCanvas.GetComponent<IDragDestination>();
            }
            else
            {
                container = GetContainer(eventData);
            }

            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }

        private IDragDestination GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination>();

                return container;
            }
            return null;
        }

        private void DropItemIntoContainer(IDragDestination destination)
        {
            if (ReferenceEquals(destination, source)) 
                return;

            var destinationContainer = destination as IDragContainer;
            var sourceContainer = source as IDragContainer;

            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }
        }

        private bool AttemptSimpleTransfer(IDragDestination destination)
        {
            var draggingItem = source.GetItem();
            
            if (draggingItem != null)
            {
                source.RemoveItem();
                destination.AddItem(draggingItem);
                return false;
            }

            return true;
        }
    }
}