using Assets.Scripts.item;
using Assets.Scripts.item.interfaces;
using Assets.Scripts.scene.inrefaces;
using Assets.Scripts.utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectItemScript : MonoBehaviour, IRectItem, IDragHandler
{
    
    Dictionary<IRectItem, IRectItemsLink> _links = new Dictionary<IRectItem, IRectItemsLink>();

    ISceneManager sceneManager;

    RectTransform _rectTransform;
    public RectTransform rectTransform => _rectTransform;

    void Awake()
    {
        sceneManager = SceneManager.Instance;

        _rectTransform = gameObject.GetComponentInChildren<RectTransform>();

        Image image = gameObject.GetComponentInChildren<Image>();
        image.color = ColorUtil.GetRandomColor();
    }

    public bool TryGetLinkToItem(IRectItem toItem, out IRectItemsLink itemsLink)
    {
        return _links.TryGetValue(toItem, out itemsLink);
    }

    public void AddLink(IRectItem toItem, IRectItemsLink link)
    {
        _links.Add(toItem, link);
    }

    public void RemoveLink(IRectItem toItem)
    {
        _links.Remove(toItem);
    }

    public void Destroy()
    {
        Destroy(gameObject);

        List<IRectItemsLink> links = new List<IRectItemsLink>(_links.Values);

        foreach (var link in links)
            link.DestroyLink();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            TrySetDraggedPosition(eventData);
    }

    void TrySetDraggedPosition(PointerEventData data)
    {
        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
        {
            RectTransform rectTransform = data.pointerEnter.transform as RectTransform;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out Vector3 globalMousePos))
            {
                if (sceneManager.CanDragItemToPosition(this, globalMousePos))
                {
                    _rectTransform.position = globalMousePos;

                    foreach (var link in _links.Values)
                        link.UpdateTransformFromItems();
                }
            }
        }
    }
}
