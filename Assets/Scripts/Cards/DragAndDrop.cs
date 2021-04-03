using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float alpha = 0.6f;
    [SerializeField] private float duration = 0.6f;
    
    [SerializeField] private Canvas canvas;
    
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Vector2 startPosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup =  GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(alpha, duration);

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, duration);
        rectTransform.DOAnchorPos(startPosition, duration);
    }
}
