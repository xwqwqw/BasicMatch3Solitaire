using System.Collections.Generic;
using _Main.Scripts.Commands;
using _Main.Scripts.Controllers;
using _Main.Scripts.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Scripts.CardSystem
{
    public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private  TextMeshProUGUI cardIdTmp;
        public string CardId { get; private set; }
        public CardStack CurrentStack { get; set; }

        private Vector3 _startPos;
        private Transform _originalParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CurrentStack == null || CurrentStack.TopCard != this) return; 
        
            _startPos = transform.position;
            _originalParent = transform.parent;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CurrentStack == null || CurrentStack.TopCard != this) return; 

            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (CurrentStack == null || CurrentStack.TopCard != this) return; 

            if (TryGetStackUnderMouse(out var newStack) && newStack != CurrentStack)
            {
                var cmd = new MoveCardCommand(this, CurrentStack, newStack);
                UndoController.Instance.ExecuteCommand(cmd);
            }
            else
            {
                transform.position = _startPos;
                transform.SetParent(_originalParent);
            }
        }

        private bool TryGetStackUnderMouse(out CardStack stack)
        {
            stack = null;
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, raycastResults);

            foreach (var result in raycastResults)
            {
                if (!result.gameObject.TryGetComponent(out CardStack foundStack)) continue;
            
                stack = foundStack;
                return true;
            }

            return false;
        }
    
        public void SetCardData(CardData data)
        {
            CardId = data.cardId;
            cardIdTmp.text = CardId;
        }
    }
}