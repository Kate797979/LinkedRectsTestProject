using Assets.Scripts.item;
using Assets.Scripts.item.interfaces;
using Assets.Scripts.scene.inrefaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

public class SceneManager : MonoBehaviour, ISceneManager, IPointerClickHandler
{

    public static ISceneManager Instance { get; private set; }

    public GameObject itemPrefab;
    public GameObject linkLinePrefab;

    GameObject _itemsPanel;
    GameObject _linksPanel;

    Vector2 _itemSize;
    Rect _itemAvailablePositionRect;

    List<IRectItem> _items = new List<IRectItem>();

    IRectItem _itemDoubleClickIgnore;

    bool _linkMode = false;
    IRectItemsLink _itemsLinkPoiter;
    IRectItem _linkItemSelected;

    void Awake()
    {
        Instance = this;

        _itemSize = itemPrefab.GetComponentInChildren<BoxCollider2D>().size;

        _itemsPanel = GameObject.FindGameObjectWithTag("ItemsPanel");
        _linksPanel = GameObject.FindGameObjectWithTag("ItemsLinksPanel");

        //Создаём линию для режима создания (удаления) связи
        var itemsLinkGameObject = Instantiate(linkLinePrefab, _linksPanel.transform, false);
        _itemsLinkPoiter = itemsLinkGameObject.GetComponent<IRectItemsLink>();
        _itemsLinkPoiter.gameObject.SetActive(false);
        _itemsLinkPoiter.SetAlpha(0.5f);
        //

        //
        Camera camera = Camera.main;
        _itemAvailablePositionRect = new Rect(_itemSize.x / 2, _itemSize.y / 2, camera.pixelWidth - _itemSize.x, camera.pixelHeight - _itemSize.y);
        //
    }

    void Update()
    {
        if (_linkMode)
            _itemsLinkPoiter.UpdateTransformFromLinePoints(_linkItemSelected.rectTransform.position, Input.mousePosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject gameObjectClicked = eventData.pointerCurrentRaycast.gameObject;
        IRectItem rectItemClicked = gameObjectClicked.GetComponentInParent<IRectItem>();

        switch (eventData.button)
        {
            case InputButton.Right:

                if (eventData.clickCount == 1 && rectItemClicked != null)
                {
                    if (_linkItemSelected != null)
                    {
                        if (rectItemClicked != _linkItemSelected)//Можно создать связь или удалить, если она уже существует
                        {
                            if (rectItemClicked.TryGetLinkToItem(_linkItemSelected, out IRectItemsLink itemsLink))
                                itemsLink.DestroyLink();
                            else
                                CreateItemsLink(_linkItemSelected, rectItemClicked);
                        }
                    }
                    else
                    {
                        _linkItemSelected = rectItemClicked;

                        SetItemLinkMode(true);

                        return;
                    }
                }

                SetItemLinkMode(false);

                break;

            case InputButton.Left:

                if (_linkMode)
                {
                    SetItemLinkMode(false);
                    return;
                }

                if (eventData.clickCount == 2 && rectItemClicked != null && _itemDoubleClickIgnore != rectItemClicked)
                {
                    RemoveItem(rectItemClicked);
                }
                else
                {
                    _itemDoubleClickIgnore = null;

                    if (eventData.clickCount == 1 && rectItemClicked == null)
                    {
                        RectTransform rectTransform = eventData.pointerEnter.transform as RectTransform;

                        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
                        {
                            //Проверяем, можно ли добавить прямоугольник, и если да, то добавляем
                            if (CanCreateItemInPosition(globalMousePos))
                                _itemDoubleClickIgnore = CreateItem(globalMousePos);//Необходимо, чтобы не удалить сразу же только что созданный прямоугольник
                        }
                    }
                }

                break;

            default:

                SetItemLinkMode(false);

                break;
        }
    }

    void SetItemLinkMode(bool active)
    {
        _linkMode = active;
        _itemsLinkPoiter.gameObject.SetActive(active);

        if (!active)
            _linkItemSelected = null;
    }

    IRectItemsLink CreateItemsLink(IRectItem item1, IRectItem item2)
    {
        var itemsLinkGameObject = Instantiate(linkLinePrefab, _linksPanel.transform, false);
        IRectItemsLink itemsLink = itemsLinkGameObject.GetComponent<IRectItemsLink>();

        itemsLink.CreateLink(item1, item2);

        return itemsLink;
    }

    bool AreRectsIntersects(Vector3 centerPosition1, Vector3 centerPosition2, Vector2 rectSize)
    {
        return Mathf.Abs(centerPosition1.x - centerPosition2.x) < rectSize.x && Mathf.Abs(centerPosition1.y - centerPosition2.y) < rectSize.y;
    }

    bool CanCreateItemInPosition(Vector3 positionToAdd)
    {
        if (IsItemOverCameraBounds(positionToAdd))
            return false;

        foreach (var item in _items)
        {
            if (AreRectsIntersects(item.rectTransform.position, positionToAdd, _itemSize))
                return false;
        }

        return true;
    }

    bool IsItemOverCameraBounds(Vector3 itemPosition)
    {
        return (itemPosition.x < _itemAvailablePositionRect.xMin || itemPosition.x > _itemAvailablePositionRect.xMax 
            || itemPosition.y < _itemAvailablePositionRect.yMin || itemPosition.y > _itemAvailablePositionRect.yMax);
    }

    IRectItem CreateItem(Vector3 position)
    {
        GameObject newItem = Instantiate(itemPrefab, _itemsPanel.transform, false);
        
        IRectItem rectItem = newItem.GetComponent<IRectItem>();
        rectItem.rectTransform.position = position;

        _items.Add(rectItem);

        return rectItem;
    }

    void RemoveItem(IRectItem item)
    {
        _items.Remove(item);
        item.Destroy();
    }
        
    public bool CanDragItemToPosition(IRectItem draggingItem, Vector3 toPosition)
    {
        if (IsItemOverCameraBounds(toPosition))
            return false;

        foreach (var item in _items)
        {
            if (draggingItem != item && AreRectsIntersects(item.rectTransform.position, toPosition, _itemSize))
                return false;
        }

        return true;
    }
}
