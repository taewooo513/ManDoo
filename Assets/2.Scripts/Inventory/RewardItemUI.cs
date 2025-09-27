using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public interface IRewardItem
{
    public void Obtain(int amount);
}
public class RewardItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggingObject, IRewardItem
{
    // IDraggingObject
    public eItemType itemType;
    public int itemId;
    public int amount;
    public InGameVictoryUI owner;
    [SerializeField] private Canvas canvas;
    private RectTransform rect;
    private Transform original;
    private CanvasGroup cg;

    public  eItemType ItemType => itemType;
    public int ItemId => itemId;
    public int Amount => amount;
    public DragOrigin Origin => DragOrigin.Reward;
    public int SlotIndex => -1;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        original = transform.parent;
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        if (cg)
        {
            cg.blocksRaycasts = false;
            cg.alpha = 0.5f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rect == null || canvas == null) return;
        var rt = canvas.transform as RectTransform;
        var camera = eventData.pressEventCamera != null ? eventData.pressEventCamera : canvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, camera, out var local))
            rect.anchoredPosition = local;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var target = FindDroppingTarget(eventData);
        if (target != null && target.Drop(this))
            Debug.Log("드롭 성공");
        else
        {
            if (original != null)
            {
                transform.SetParent(original, false);
                rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(100, 100);
                rect.localScale = Vector3.one;
            }
        }

        if (cg)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

    private IDroppingTarget FindDroppingTarget(PointerEventData eventData)
    {
        var gr = canvas
            ? canvas.GetComponentInParent<UnityEngine.UI.GraphicRaycaster>()
            : GetComponentInParent<UnityEngine.UI.GraphicRaycaster>();
        if (gr == null || EventSystem.current == null) return null;
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (var hit in hits)
        {
            var target = hit.gameObject.GetComponentInParent<IDroppingTarget>();
            if (target != null) 
                return target;
        }

        return null;
    }

    public void Obtain(int amount)
    {
        if (owner == null) return;
        owner.ObtainRewardItem(itemType, itemId, amount);
    }
}
