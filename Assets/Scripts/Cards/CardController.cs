using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    public class CardController: MonoBehaviour
    {
        [SerializeField] private int startNumCardInHand;
        [SerializeField] private int maxNumCardInHand;
        [SerializeField] private float pickCardDelay;
        
        [SerializeField] private Card[] uniqueCards;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject cardPlacePrefab;

        [SerializeField] private Vector2 cardSize = new Vector2(270,400);
        
        [SerializeField] private float selectScale = 1.2f;
        [SerializeField] private float selectMove = 100;

        [SerializeField] private float dragScale = 0.5f;
        [SerializeField] private float dragAlpha = 0.6f;
        
        [SerializeField] private float duration = 0.3f;

        [SerializeField] private Canvas canvas;
        
        private List<CardView> hand = new List<CardView>(6);
        
        public bool RequestCards = true;
        public bool Exit;
        
        
        private bool deselectAnim;
        private SelectedCard selectedCard = new SelectedCard();
        private HorizontalLayoutGroup layout;

        private void Awake()
        {
            if (canvas == null) canvas = GetComponentInParent<Canvas>();

            layout = GetComponent<HorizontalLayoutGroup>();
            for (var i = 0; i < startNumCardInHand; ++i)
            {
                TryPickCard();
            }

            StartCoroutine(Progress());
        }

        private IEnumerator Progress()
        {
            while (!Exit)
            {
                var delay = pickCardDelay;
                if (RequestCards)
                {
                    if (deselectAnim || selectedCard.view)
                    {
                        delay = duration;
                    }
                    else
                    {
                        TryPickCard();
                    }
                }
                yield return new WaitForSeconds(delay);
            }
        }
        
        private void TryPickCard()
        {
            if (hand.Count >= maxNumCardInHand) return;

            var card = PickRandomCard();
            var view = Create(card);
            
            var rectTransform = view.GetComponent<RectTransform>();
            var rectTransformParent = rectTransform.parent.GetComponent<RectTransform>();;
            
            rectTransform.sizeDelta = cardSize * dragScale;
            rectTransform.anchoredPosition = new Vector2(900, 900);
            rectTransformParent.sizeDelta = Vector2.zero;

            selectedCard.view = view;
            
            rectTransformParent.DOSizeDelta(cardSize, duration);
            rectTransform.DOSizeDelta(cardSize, duration);
            rectTransform.DOAnchorPos(Vector2.zero, duration).OnComplete(() =>
            {
                selectedCard.Clear();
            });
            
            hand.Add(view);
        }
        
        // TODO: add more complex logic to pick new card
        private Card PickRandomCard() => uniqueCards[Random.Range(0, uniqueCards.Length)];

        private CardView Create(Card card)
        {
            var placeView = Instantiate(cardPlacePrefab).transform;
            var view = Instantiate(cardPrefab).GetComponent<CardView>();
            
            view.SetParent(placeView);
            placeView.SetParent(transform);
            placeView.localScale = Vector3.one;

            view.Init(card);
            view.OnDropHandler += OnDrop;
            view.OnBeginDragHandler += OnBeginDrag;
            view.OnEndDragHandler += OnEndDrag;
            view.OnDragHandler += OnDrag;
            
            view.OnPointerEnterHandler += OnPointerEnter;
            view.OnPointerExitHandler += OnPointerExit;
            
            return view;
        }

        private void OnPointerEnter(CardView view)
        {
            if (!selectedCard.IsSelected && selectedCard.view == null)
            {
                selectedCard.IsSelected = true;  
                
                selectedCard.view = view;
                selectedCard.initialParent = view.transform.parent.GetComponent<RectTransform>();

                selectedCard.view.SetParent(canvas.transform, true);
                
                selectedCard.initialPosition = view.RectTransform.anchoredPosition;
                selectedCard.initialScale = view.RectTransform.localScale;
                
                view.RectTransform.DOScale(selectScale, duration);
                view.RectTransform.DOAnchorPosY(view.RectTransform.anchoredPosition.y + selectMove, duration);
            }
        }
        
        private void OnPointerExit(CardView view)
        {
            if (selectedCard.IsSelected && selectedCard.view == view)
            {
                DOTween.Complete(selectedCard.view);
                DOTween.Complete(selectedCard.initialParent);

                deselectAnim = true;
                
                var parent = selectedCard.initialParent;

                view.CanvasGroup.blocksRaycasts = false;
                view.RectTransform.anchoredPosition = selectedCard.initialPosition + selectMove * Vector2.up;

                view.RectTransform.DOScale(selectedCard.initialScale, duration);
                view.RectTransform.DOAnchorPos(selectedCard.initialPosition, duration).OnComplete(() => { 
                        view.CanvasGroup.blocksRaycasts = true;
                        view.SetParent(parent, true);

                        deselectAnim = false;
                });
                
                selectedCard.Clear();
            }
        }


        private void OnBeginDrag(CardView view)
        {
            if (!selectedCard.IsDragging && selectedCard.view == view)
            {
                DOTween.Complete(selectedCard.view);
                DOTween.Complete(selectedCard.initialParent);
                
                selectedCard.IsSelected = false;
                selectedCard.IsDragging = true;
                
                view.CanvasGroup.blocksRaycasts = false;

                selectedCard.initialParent.DOSizeDelta(Vector2.zero, duration);
                view.CanvasGroup.DOFade(dragAlpha, duration);
                view.RectTransform.DOScale(dragScale, duration);
            }
        }

        private void OnDrag(CardView view, Vector2 delta)
        {
            if (selectedCard.IsDragging && selectedCard.view == view)
            {
                view.CanvasGroup.blocksRaycasts = false;
                view.RectTransform.anchoredPosition += delta / canvas.scaleFactor;
            }
        }

        private void OnDrop(CardView view)
        {
            if (selectedCard.IsDragging && selectedCard.view == view)
            {
                selectedCard.IsDropped = true;
            }
        }

        private void OnEndDrag(CardView view)
        {
            if (selectedCard.IsDragging && selectedCard.view == view)
            {
                if (!selectedCard.IsDropped)
                {
                    DOTween.Complete(selectedCard.view);
                    DOTween.Complete(selectedCard.initialParent);
                    
                    view.SetParent(selectedCard.initialParent, true);
                    
                    selectedCard.initialParent.DOSizeDelta(cardSize, duration);
                    view.CanvasGroup.DOFade(1, duration);
                    view.RectTransform.DOScale(selectedCard.initialScale, duration);
                    view.RectTransform.DOAnchorPos(Vector2.zero, duration)
                        .OnComplete(() =>
                        {
                            view.CanvasGroup.blocksRaycasts = true;
                            
                            view.RectTransform.anchoredPosition = Vector2.zero;
                            
                            selectedCard.Clear();
                        });
                }
                else
                {
                    view.RectTransform.DOScale( 0, duration);
                    view.CanvasGroup.DOFade(0, duration)
                        .OnComplete(() =>
                        {
                            hand.Remove(view);

                            Destroy(view.gameObject);
                            Destroy(selectedCard.initialParent.gameObject);
                            
                            selectedCard.Clear();
                        });
                }
            }
        }

        class SelectedCard
        {
            internal CardView view;
            
            internal RectTransform initialParent;
            internal Vector2 initialPosition;
            internal Vector3 initialScale;
            
            internal bool IsDropped;
            internal bool IsDragging;
            internal bool IsSelected;
            
            public void Clear()
            {
                view = null;
                initialParent = null;
                IsDropped = false;
                IsDragging = false;
                IsSelected = false;
            }
        }
    }
}