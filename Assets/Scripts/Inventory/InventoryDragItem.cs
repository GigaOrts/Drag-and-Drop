using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _originalParent;
    private IDragSource _source;

    private Canvas _parentCanvas;

    private void Awake()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
        _source = GetComponentInParent<IDragSource>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _originalParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(_parentCanvas.transform, true);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.position = _startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(_originalParent, true);

        IDragDestination container;

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            container = _parentCanvas.GetComponent<IDragDestination>();
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
        if (ReferenceEquals(destination, _source))
            return;

        var destinationContainer = destination as IDragContainer;
        var sourceContainer = _source as IDragContainer;

        if (destinationContainer == null || sourceContainer == null || 
            destinationContainer.GetItem() == null ||
            ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
        {
            AttemptSimpleTransfer(destination);
            return;
        }
    }

    private bool AttemptSimpleTransfer(IDragDestination destination)
    {
        var draggingItem = _source.GetItem();

        if (draggingItem != null)
        {
            _source.RemoveItem();
            destination.AddItem(draggingItem);
            return false;
        }

        return true;
    }
}