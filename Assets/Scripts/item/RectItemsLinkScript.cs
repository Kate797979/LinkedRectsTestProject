using Assets.Scripts.item;
using Assets.Scripts.item.interfaces;
using Assets.Scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class RectItemsLinkScript : MonoBehaviour, IRectItemsLink
{

    RectTransform _rectTransform;
    public RectTransform rectTransform => _rectTransform;

    IRectItem _item1;
    public IRectItem item1 => _item1;

    IRectItem _item2;
    public IRectItem item2 => _item2;

    public void CreateLink(IRectItem item1, IRectItem item2)
    {
        _item1 = item1;
        _item1.AddLink(item2, this);

        _item2 = item2;
        _item2.AddLink(item1, this);

        UpdateTransformFromItems();
    }

    public void DestroyLink()
    {
        if (_item1 != null && _item2 != null)
        {
            _item1.RemoveLink(_item2);
            _item2.RemoveLink(_item1);

            _item1 = _item2 = null;

            Destroy(gameObject);
        }
    }

    public void UpdateTransformFromItems()
    {
        UpdateTransformFromLinePoints(_item1.rectTransform.position, _item2.rectTransform.position);
    }

    public void UpdateTransformFromLinePoints(Vector3 lineStartPosution, Vector3 lineEndPosution)
    {
        (float length, Quaternion angle) = Line2DUtil.Get2DLineParams(lineStartPosution, lineEndPosution);

        _rectTransform.position = lineStartPosution;
        _rectTransform.sizeDelta = new Vector2(length, rectTransform.rect.height);
        _rectTransform.rotation = angle;
    }

    public void SetAlpha(float alpha)
    {
        Image image = gameObject.GetComponentInChildren<Image>();

        Color imageColor = image.color;
        imageColor.a = alpha;

        image.color = imageColor;
    }

    void Awake()
    {
        _rectTransform = gameObject.GetComponentInChildren<RectTransform>();
    }

}
